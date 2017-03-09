package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.bean.JsonBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.JsonUtil;

/**
 * 提交採購調撥申請至JDA接口端
 * @author s1713 modify s2139
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_105_02 extends BusinessService implements ProcessUnit {

	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
			throw new RuntimeException();
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht105.02提交采购调拨申请至JDA接口端, response="+bean.getResponse().toString());

//		log.info("response=" + bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {

		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil
				.getValue(bean.getUser().getStore(),"hht105.02.03"), new Object[] { bean.getRequest()
				.getParameter(configpropertyUtil.getValue("bc")) });
		//		
		// Root r=new Root();
		if (list.size() < 1) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("该单号不存在。");
		}else if(list.get(0).get("HHTSTS").toString().equals("1")){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("请至修改菜单维护。");
		}else if(!(list.get(0).get("HHTSTS").toString().equals("A"))){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("该单号的状态不可做新增。");
		}else {
			
			//判断箱号是否已经上传JDA，如果已上传到JDA，则返回错误信息，不允许再次提交
			List<Map> list_ = jdbcTemplateUtil.searchForList(sqlpropertyUtil
					.getValue(bean.getUser().getStore(),"hht105.02.04"), new Object[] { bean.getRequest()
					.getParameter(configpropertyUtil.getValue("bc")) });
			if(list_.size()>0){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.105.02"));
				return ;
			}
			
			String sysId = "";
			String username = "";
			String store = "";
			if (bean.getUser() != null) {
				sysId = bean.getUser().getStore() + this.systemUtil.getSysid();
				username = bean.getUser().getName();
				store = bean.getUser().getStore();
			}else{
				throw new Exception("当前用户已登出，或系统异常，找不到操作用户");
			}
//			String sysId = bean.getUser()!=null?bean.getUser().getStore() + this.systemUtil.getSysid():this.systemUtil.getSysid();
//			String sysId = "";
//			if (bean.getUser() != null) {
//				sysId = bean.getUser().getStore() + this.systemUtil.getSysid();
//			} else {
//				sysId = this.systemUtil.getSysid();
//			}
//			String username = bean.getUser()!=null?bean.getUser().getName():"s1777";
//			String username = "";
//			if (bean.getUser() != null) {
//				username = bean.getUser().getName();
//			} else {
//				username = "s1777";
//			}
//			String store = bean.getUser()!=null?bean.getUser().getStore():"13101";
//			String store = "";
//			if (bean.getUser() != null) {
//				store = bean.getUser().getStore();
//			} else {
//				store = "13101";
//			}

			int cnt = 0;

			List l = new ArrayList();
			l.add(configpropertyUtil.getValue("hht001nodeName.01"));
			// l.add(configpropertyUtil.getValue("hht001nodeName.02"));
			JsonBean json = JsonUtil.jsonToList(String.valueOf(bean.getRequest()
					.getParameter(configpropertyUtil.getValue("xml"))), l);

			Long D1SHQT;
			String sku = "";
			String reason = "";// 退货原因
			String numid = this.getSystemUtil().getNumId(store);
//			String hhthno = numid.substring(numid.length() - 10, numid.length());
			String vendorcode = "";// 厂商代码
			String usercode = "";
			
			//箱号
			String hhtcno = this.systemUtil.getNumId(store);
			
			Map data[] = json.getData().get(
					configpropertyUtil.getValue("hht001nodeName.01"));
			String sql[] = new String[data.length + 1];
			Object o[] = new Object[data.length + 1];
			
			for (Map m : data) {
				D1SHQT = new Long(String.valueOf(m.get(configpropertyUtil
						.getValue("HHTQTY"))));
				
				sku = String.valueOf(m.get(configpropertyUtil.getValue("sku")));
				
				reason = String.valueOf(m.get(configpropertyUtil
						.getValue("reason")));
				if(reason.equals("null"))
					reason = "";

				vendorcode = String.valueOf(m.get(configpropertyUtil
						.getValue("vendorcode")));
				
				usercode = String.valueOf(m.get(configpropertyUtil
						.getValue("usercode")));
				
				String hhtno = String.valueOf(m.get("RTV"));
				
				sql[cnt] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht105.02.01");
				o[cnt] = new Object[] { sysId, sku, D1SHQT, reason,
						hhtcno,hhtno
						};

				cnt++;
			}

			if (list != null && list.size() > 0) {
				Map m = list.get(0);
				sql[cnt] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht105.02.02");
				if(m.get("HHTVND")==null ||  m.get("HHTVND").toString().length()==0){
					o[cnt] = new Object[] {sysId, m.get("HHTNO"), m.get("HHTTYP"),
							m.get("HHTSTS"), m.get("HHTFLC"), m.get("HHTTLC"),
							m.get("HHTOCD"), m.get("HHTRCW"), m.get("HHTPNM"),
							m.get("HHTEDT"), m.get("HHTEDT"), m.get("HHTMA1"),
							m.get("HHTMA2"), m.get("HHTESY"), hhtcno, username,
							username, m.get("HHTTLC"), m.get("HHTPCD")};
				}else{
					o[cnt] = new Object[] {sysId, m.get("HHTNO"), m.get("HHTTYP"),
							m.get("HHTSTS"), m.get("HHTFLC"), m.get("HHTTLC"),
							m.get("HHTOCD"), m.get("HHTRCW"), m.get("HHTPNM"),
							m.get("HHTEDT"), m.get("HHTEDT"), m.get("HHTMA1"),
							m.get("HHTMA2"), m.get("HHTESY"), hhtcno, username,
							username, m.get("HHTVND"), m.get("HHTPCD")};
				}
				
			}
//			this.jdbcTemplateUtil.update(sql, o);
			String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
			String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
			super.updateForValidation(sql, o, bean.getUser().getStore(), guid, requestValue, hhtcno);
//			super.update(bean.getRequest().getRequestID(), sysId, sql, o,bean.getUser().getStore() );
			//返回箱号
			bean.getResponse().setDesc(hhtcno);
		}

	}
}

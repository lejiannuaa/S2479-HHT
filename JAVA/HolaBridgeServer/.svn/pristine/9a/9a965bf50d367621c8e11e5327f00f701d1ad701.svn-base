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
 * RTV退货至大仓（门店发起）
 * @author s1713 modify s2139
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_103_03 extends BusinessService implements ProcessUnit {

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
		log.info("operation hht103.03门店发起退货至大仓, response="+bean.getResponse().toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil
				.getValue(bean.getUser().getStore(), "hht103.03"), new Object[] { bean.getRequest().getParameter(
				configpropertyUtil.getValue("bc")) });

		// Root r = new Root();
		if ("0".equals(list.get(0).get("cnt"))) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.01"));
		} else {
			
			// String s = String.valueOf(list.get(0).get("D1STAT"));
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
//			if (bean.getUser() != null) {
//				sysId = bean.getUser().getStore() + this.systemUtil.getSysid();
//			} else {
//				sysId = this.systemUtil.getSysid();
//			}
//			
//			if (bean.getUser() != null) {
//				username = bean.getUser().getName();
//			} else {
//				username = "s1777";
//			}
//			
//			if (bean.getUser() != null) {
//				store = bean.getUser().getStore();
//			} else {
//				store = "13101";
//			}

			// 箱号
			String numid = "";
			this.getSystemUtil().getNumId(store);
			int cnt = 0;

			List l = new ArrayList();
			l.add(configpropertyUtil.getValue("hht001nodeName.01"));
			// l.add(configpropertyUtil.getValue("hht001nodeName.02"));
			JsonBean json = JsonUtil.jsonToList(String.valueOf(bean.getRequest().getParameter(
					configpropertyUtil.getValue("xml"))), l);

			Long D1SHQT = null;// 出货数量
			String sku = "";
			String reason = "";// 退货原因
			String code = "";// 店代码
			String vendorcode = "";// 厂商代码
			String usercode = "";

			Map data[] = json.getData().get(configpropertyUtil.getValue("hht001nodeName.01"));
			String sql[] = new String[data.length + 3];
			Object o[] = new Object[data.length + 3];

			sql[0] = sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht103.03.03");
			o[0] = new Object[] { bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) };
			sql[1] = sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht103.03.04");
			o[1] = new Object[] { bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) };

			if (bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) != null
					&& !"".equals(bean.getRequest().getParameter(configpropertyUtil.getValue("bc")))) {
				numid = String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("bc")));
			} else {
				numid = this.getSystemUtil().getNumId(store);
			}
			cnt = 2;
			for (Map m : data) {
				D1SHQT = new Long(String.valueOf(m.get(configpropertyUtil.getValue("HHTQTY"))));
				sku = String.valueOf(m.get(configpropertyUtil.getValue("sku")));
				reason = String.valueOf(m.get(configpropertyUtil.getValue("reason")));

				vendorcode = String.valueOf(m.get(configpropertyUtil.getValue("vendorcode")));
				usercode = String.valueOf(m.get(configpropertyUtil.getValue("usercode")));
				sql[cnt] = sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht103.03.01");
				o[cnt] = new Object[] { sysId, sku, D1SHQT, reason, numid,
						sysId.substring(sysId.length() - 10, sysId.length()) };

				cnt++;
			}

			// 保存head信息
			sql[cnt] = sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht103.03.02");
			o[cnt] = new Object[] { store, vendorcode, sysId.substring(sysId.length() - 10, sysId.length()), username,
					numid, username, sysId };
			// this.jdbcTemplateUtil.update(sql, o);

			// 设定箱号并返回
			bean.getResponse().setDesc(numid);
			String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
			String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
			super.update(bean.getRequest().getRequestID(), sysId, sql, o, bean.getUser().getStore(),guid,requestValue,numid);
		}

	}
}

package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.property.PullDownUtils;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Root;

/**
 * 整箱破損
 * 
 * @author s1713 modify s2139
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_001_04 extends BusinessService implements ProcessUnit {
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
		// log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht001.04整箱破損, response="
				+ bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean){
			List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil
					.getValue(bean.getUser().getStore(), "hht001.04"),
					new Object[] { bean.getRequest().getParameter(
							configpropertyUtil.getValue("bc")) });

			Root r = new Root();
			if (list.size() < 1) {
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(
						configpropertyUtil.getValue("msg.01"));
			} else {

				String s = String.valueOf(list.get(0).get("HHTSTA"));
				String crtusr = list.get(0).get("CRTUSER").toString();
				if (s.trim().equals("2")) {
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc(
							"该箱状态为" + PullDownUtils.getHHTStatusList().get(s)
									+ "，不可整箱破损");
				} else if (s.trim().equals("1")
						&& !crtusr.equals(bean.getUser().getName())) {
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc("该箱正由用户：" + crtusr + "执行收货中");
				} else {
					int sqlSize = 2;// 2+json.getData().get(configpropertyUtil.getValue("hht001nodeName.01")).length;
					String hhtflc = list.get(0).get("HHTFLC").toString();
					String hhttlc = list.get(0).get("HHTTLC").toString();
					List<Map> data = jdbcTemplateUtil.searchForList(
							sqlpropertyUtil.getValue(bean.getUser().getStore(),
									"hht001.04.04"),
							new Object[] { bean.getRequest().getParameter(
									configpropertyUtil.getValue("bc")) });

					if (data != null) {
						sqlSize += data.size();
					}

					// String sysId="";
					String sysId = bean.getUser() != null ? bean.getUser()
							.getStore()
							+ this.systemUtil.getSysid() : this.systemUtil
							.getSysid();
					// if(bean.getUser()!=null){
					// sysId=bean.getUser().getStore()+
					// this.systemUtil.getSysid();
					// }else{
					// sysId=this.systemUtil.getSysid();
					// }
					String sql[] = new String[sqlSize];

					Object o[] = new Object[sqlSize];
					sql[0] = sqlpropertyUtil.getValue(
							bean.getUser().getStore(), "hht001.04.01");
					o[0] = new Object[] {
							sysId,
							bean.getRequest().getParameter(
									configpropertyUtil.getValue("bc")) };
					int i = 1;
					for (Map tmp : data) {
						sql[i] = sqlpropertyUtil.getValue(bean.getUser()
								.getStore(), "hht001.04.02");
						o[i] = new Object[] {
								sysId,
								bean.getRequest().getParameter(
										configpropertyUtil.getValue("bc")),
								tmp.get("HHTSKU"), tmp.get("HHTPQT"), hhtflc,
								hhttlc };
						// bean.getUser().getAttribute("HHTFLC"),
						// bean.getUser().getAttribute("HHTTLC")};
						i++;
					}
					sql[sqlSize - 1] = sqlpropertyUtil.getValue(bean.getUser()
							.getStore(), "hht001.04.03");
					// String username = bean.getUser() !=
					// null?bean.getUser().getName():"s1777";
					String username = "";
					if (bean.getUser() != null) {
						username = bean.getUser().getName();
					} else {
						try {
							throw new Exception("当前用户已登出，或系统异常，找不到操作用户");
						} catch (Exception e) {
						}
					}
					o[sqlSize - 1] = new Object[] {
							sysId,
							username,
							bean.getRequest().getParameter(
									configpropertyUtil.getValue("bc")) };
					// this.jdbcTemplateUtil.update(sql, o);
					String guid = bean.getRequest().getParameter(
							configpropertyUtil.getValue("guid"));
					String requestValue = bean.getRequest().getParameter(
							configpropertyUtil.getValue("requestValue"));
					super.update(bean.getRequest().getRequestID(),
							sysId, sql, o, bean.getUser().getStore(), guid,
							requestValue, "");
				}

			}

	}
}

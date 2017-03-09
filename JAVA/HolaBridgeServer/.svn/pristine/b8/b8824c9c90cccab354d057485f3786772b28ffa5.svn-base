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
 * 
 * @author s1713
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_107_02 extends BusinessService implements ProcessUnit {

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
		log.info("operation hht107.02, response="
				+ bean.getResponse().toString());

		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {

		int cnt = 0;
		List l = new ArrayList();
		l.add(configpropertyUtil.getValue("hht001nodeName.01"));
		// l.add(configpropertyUtil.getValue("hht001nodeName.02"));
		JsonBean json = JsonUtil.jsonToList(String.valueOf(bean.getRequest()
				.getParameter(configpropertyUtil.getValue("xml"))), l);

		String D1SHQT = "";
		String sku = "";
		String reason = "";// 退货原因
		String code = "";// 店代码
		String incode = "";// '店代码'
		String usercode = "";
		String number = "";// 数量
		String thsl = "";// 退货数量
		String xh = "";// 箱号
		Map data[] = json.getData().get(
				configpropertyUtil.getValue("hht001nodeName.01"));
		String sql[] = new String[data.length];
		Object o[] = new Object[data.length];

		for (Map m : data) {
			D1SHQT = String.valueOf(m
					.get(configpropertyUtil.getValue("D1SHQT")));
			sku = String.valueOf(m.get(configpropertyUtil.getValue("sku")));
			reason = String.valueOf(m
					.get(configpropertyUtil.getValue("reason")));
			code = String.valueOf(m.get(configpropertyUtil.getValue("code")));
			incode = String.valueOf(m.get(configpropertyUtil
					.getValue("vendorcode")));
			usercode = String.valueOf(m.get(configpropertyUtil
					.getValue("usercode")));
			thsl = String.valueOf(m.get(configpropertyUtil.getValue("thsl")));
			xh = String.valueOf(m.get(configpropertyUtil.getValue("xh")));
			// sku=String.valueOf(m.get(configpropertyUtil.getValue("nodeSKU")));
			sql[cnt] = sqlpropertyUtil.getValue(bean.getUser().getStore(),
					"hht107.02.01");
			o[cnt] = new Object[] { thsl, m.get("RTV"), xh, sku };
			cnt++;
		}

		// this.jdbcTemplateUtil.update(sql, o);
		String guid = bean.getRequest().getParameter(
				configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(
				configpropertyUtil.getValue("requestValue"));
		super.updateForValidation(sql, o, bean.getUser().getStore(), guid,
				requestValue, "");

	}
}

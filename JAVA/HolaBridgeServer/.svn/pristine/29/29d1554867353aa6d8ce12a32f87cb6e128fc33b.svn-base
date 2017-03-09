package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.commons.lang.StringUtils;
import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.bean.JsonBean;
import com.hola.bs.bean.Response;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.JsonUtil;

/**
 * bridgeserver端店铺出货到厂商的操作
 * 
 * @author s1713 modify by s2139
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_101_04 extends BusinessService implements ProcessUnit {
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			Response r = bean.getResponse();
			r.setCode(BusinessService.errorcode);
			r.setDesc("系统错误，请联系管理员。 " + e.getMessage());
			log.error("系统错误日志", e);
			throw new RuntimeException();
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht101.04店铺出货到厂商的商品提交信息, response="
				+ bean.getResponse().toString());

		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) {
		String sysId = "";
		// String sysId = bean.getUser() != null?bean.getUser().getStore() +
		// "54301" : "54301";
		String username = null;
		String store = null;
		if (bean.getUser() != null) {
			username = bean.getUser().getName();
			store = bean.getUser().getStore();
			sysId = bean.getUser().getStore() + this.systemUtil.getSysid();
		} else {
			try {
				throw new Exception("101_04:找不到登录用户");
			} catch (Exception e) {
			}
		}

		// 箱号
		String numid = this.systemUtil.getNumId(store);
		int cnt = 0;

		// System.out.println("test submit....."+ (++Init.test_id));

		List l = new ArrayList();
		l.add(configpropertyUtil.getValue("hht001nodeName.01"));
		// l.add(configpropertyUtil.getValue("hht001nodeName.02"));
		JsonBean json = null;
		try {
			json = JsonUtil.jsonToList(String.valueOf(bean.getRequest()
					.getParameter(configpropertyUtil.getValue("xml"))), l);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		Long D1SHQT;
		String sku = "";
		String reason = "";// 退货原因

		String vendorcode = "";// 厂商代码
		String usercode = "";

		String[] deleteSqls = new String[2];
		Object[] oo = new Object[2];
		String deleteSql = sqlpropertyUtil.getValue(store, "hht101.04.04");
		deleteSqls[0] = deleteSql;
		oo[0] = new Object[] { bean.getRequest().getParameter(
				configpropertyUtil.getValue("bc")) };
		deleteSqls[1] = sqlpropertyUtil.getValue(store, "hht101.04.03");
		oo[1] = new Object[] { bean.getRequest().getParameter(
				configpropertyUtil.getValue("bc")) };
		 super.newUpdate(deleteSqls,oo);

		// 删除完后做新增
		Map data[] = json.getData().get(
				configpropertyUtil.getValue("hht001nodeName.01"));
		String sql[] = new String[data.length + 1];
		// String sql[] = new String[data.length+1];
		Object o[] = new Object[data.length + 1];
		// Object o[] = new Object[data.length+1];
		cnt = 0;

		//		
		// sql[0] = sqlpropertyUtil.getValue(store,"hht101.04.03");
		// o[0] = new Object[] {
		// bean.getRequest().getParameter(configpropertyUtil
		// .getValue("bc")) };
		//
		// sql[1] = sqlpropertyUtil.getValue(store,"hht101.04.04");
		// o[1] = new Object[] {
		// bean.getRequest().getParameter(configpropertyUtil
		// .getValue("bc")) };

		if (bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) != null
				&& !"".equals(bean.getRequest().getParameter(
						configpropertyUtil.getValue("bc")))) {
			numid = String.valueOf(bean.getRequest().getParameter(
					configpropertyUtil.getValue("bc")));
		}

		// HHT单号
		String hhtno = sysId.substring(sysId.length() - 10, sysId.length());
		for (Map m : data) {
			D1SHQT = new Long(String.valueOf(m.get(configpropertyUtil
					.getValue("HHTQTY"))));
			sku = String.valueOf(m.get(configpropertyUtil.getValue("sku")));
			reason = String.valueOf(m
					.get(configpropertyUtil.getValue("reason")));

			vendorcode = String.valueOf(m.get(configpropertyUtil
					.getValue("vendorcode")));
			vendorcode = fullVendor(vendorcode);
			// System.out.println("==============================="+vendorcode);
			usercode = String.valueOf(m.get(configpropertyUtil
					.getValue("usercode")));
			sql[cnt] = sqlpropertyUtil.getValue(store, "hht101.04.01");
			// System.out.println("HHTCNO: "
			// + sysId.substring(sysId.length() - 10, sysId.length()));
			o[cnt] = new Object[] { sysId, sku, D1SHQT, reason, numid, hhtno };

			// 设定箱号并返回
			bean.getResponse().setDesc(numid);

			cnt++;
		}

		// sql[cnt] =
		// sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht101.04.02");

		String hhtvtp = bean.getUser().getAttribute("HHTVTP");
		if (hhtvtp != null) {
			if (hhtvtp.equals("1")) {
				sql[cnt] = sqlpropertyUtil.getValue(store, "hht101.04.02.01");
				o[cnt] = new Object[] { store, vendorcode, hhtno, username,
						sysId, username, numid, vendorcode };
			} else if (hhtvtp.equals("3")) {
				sql[cnt] = sqlpropertyUtil.getValue(store, "hht101.04.02.02");
				o[cnt] = new Object[] { store, hhtno, username, sysId,
						username, numid, vendorcode };
			}
		}
		String guid = bean.getRequest().getParameter(
				configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(
				configpropertyUtil.getValue("requestValue"));
		super.update(bean.getRequest().getRequestID(), sysId, sql, o, store,
				guid, requestValue, numid);

	}

	private String fullVendor(String vendor) {
		return StringUtils.leftPad(vendor, 6, '0');
	}

}

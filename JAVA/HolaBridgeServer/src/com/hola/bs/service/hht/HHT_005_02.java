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
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;
import com.hola.bs.util.Root;

/**
 * HHT端確認PO單收貨
 * 
 * @author s1713 modify s2139
 * 
 */
@Transactional(propagation = Propagation.REQUIRED, rollbackFor = Exception.class)
public class HHT_005_02 extends BusinessService implements ProcessUnit {

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
		log.info("operation hht005.02PO單收貨確認, response="
				+ bean.getResponse().toString());

		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) {
		String sto = bean.getUser().getStore();
		String bc = bean.getRequest().getParameter(
				configpropertyUtil.getValue("bc"));
		while (bc.length() < 6) {
			bc = "0" + bc;
		}
		List<Map> list = jdbcTemplateUtil.searchForList(
				sqlpropertyUtil.getValue(sto, "hht005.02.01"),
				new Object[] { bc });

		Root r = new Root();
		if (list.size() < 1) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.01"));
		} else {
			String s = String.valueOf(list.get(0).get("D1STAT"));
			if (s != null && (s.trim().equals("3") || s.trim().equals("4"))) {

				String username = "";
				String sysId = "";
				if (bean.getUser() != null) {
					username = bean.getUser().getName();
					sysId = sto + this.systemUtil.getSysid();
				} else {
					try {
						throw new Exception("当前用户已登出，或系统异常，找不到操作用户");
					} catch (Exception e) {
					}
				}

				List l = new ArrayList();
				l.add(configpropertyUtil.getValue("hht001nodeName.01"));
				l.add(configpropertyUtil.getValue("hht001nodeName.02"));
				JsonBean json = null;
				try {
					json = JsonUtil.jsonToList(
							String.valueOf(bean.getRequest().getParameter(
									configpropertyUtil.getValue("xml"))), l);
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

				String D1SHQT = "";
				String sku = "";
				Map data[] = json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.01"));
				Map data1[] = json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.02"));
				String sql[] = new String[data.length + data1.length + 1];
				Object o[] = new Object[data.length + data1.length + 1];
				int cnt = 0;
				for (Map m : data) {
					// D1SHQT=String.valueOf(m.get(configpropertyUtil.getValue("D1SHQT")));
					// sku=String.valueOf(m.get(configpropertyUtil.getValue("num")));
					sql[cnt] = sqlpropertyUtil.getValue(sto, "hht005.02.02");
					o[cnt] = new Object[] {
							m.get(configpropertyUtil.getValue("whsl")), sysId,
							bc, m.get(configpropertyUtil.getValue("nodeSKU")) };// D1INST=’生成值’未知

					cnt++;
				}
				for (Map m : data1) {
					sql[cnt] = sqlpropertyUtil.getValue(sto, "hht005.02.04");
					o[cnt] = new Object[] { sysId, bc,
							m.get(configpropertyUtil.getValue("reason")),
							m.get(configpropertyUtil.getValue("nodeSKU")),
							m.get(configpropertyUtil.getValue("nodecount")),
							bean.getUser().getAttribute("D1STLC"),
							bean.getUser().getAttribute("D1TOLC") };// D1INST=’生成值’未知
					cnt++;
				}

				sql[cnt] = sqlpropertyUtil.getValue(sto, "hht005.02.03");
				o[cnt] = new Object[] { sysId, bean.getUser().getName(), bc };

				String guid = bean.getRequest().getParameter(
						configpropertyUtil.getValue("guid"));
				String requestValue = bean.getRequest().getParameter(
						configpropertyUtil.getValue("requestValue"));
				// super.update(bean.getRequest().getRequestID(), sysId,
				// sql,
				// o,bean.getUser().getStore(),guid,requestValue,"");

				System.out.println("HHT_005_02-1-----------〉计时开始时间："
						+ DateUtils.DateFormatToString(
								DateUtils.getSystemDate(),
								"yyyy-MM-dd HH:mm:ss.SSS") + "    "
						+ System.nanoTime());
				super.update(bean.getRequest().getRequestID(), sysId, sql, o,
						sto, guid, requestValue, "");
				System.out.println("HHT_005_02-1-----------〉计时结束时间："
						+ DateUtils.DateFormatToString(
								DateUtils.getSystemDate(),
								"yyyy-MM-dd HH:mm:ss.SSS") + "    "
						+ System.nanoTime());
				// 查询是否存在sku，如果存在，则更新数量，如果不存在，则新增数据
				int sqlSize = json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.01")).length;
				String sql_Htob1f[] = new String[sqlSize];
				Object o_Htob1f[] = new Object[sqlSize];
				cnt = 0;
				for (Map m : json.getData().get(
						configpropertyUtil.getValue("hht001nodeName.01"))) {
					sku = m.get(configpropertyUtil.getValue("nodeSKU"))
							.toString();

					List<Map> list3 = jdbcTemplateUtil.searchForList(
							sqlpropertyUtil.getValue(sto, "hht005.02.08"),
							new Object[] { sku });

					if (list3.size() == 0) {
						sql_Htob1f[cnt] = sqlpropertyUtil.getValue(sto,
								"hht005.02.06");
						o_Htob1f[cnt] = new Object[] { sysId, sto, sku,
								m.get("whsl") };

					} else {
						List<Map> list2 = jdbcTemplateUtil.searchForList(
								sqlpropertyUtil.getValue(sto, "hht005.02.05"),
								new Object[] { bc, sku });

						sql_Htob1f[cnt] = sqlpropertyUtil.getValue(sto,
								"hht005.02.07");
						o_Htob1f[cnt] = new Object[] {
								list2.get(0).get("hhtpqt"), sku };
					}
					cnt++;
				}
				this.jdbcTemplateUtil.update(sql_Htob1f, o_Htob1f);
			} else {
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(
						configpropertyUtil.getValue("msg.03.01") + s
								+ configpropertyUtil.getValue("msg.03.03"));
			}

		}
	}

}

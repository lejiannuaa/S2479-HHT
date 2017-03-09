package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * 营业课--价格查询--商品基本信息检索
 * 
 * @author S2139 2012 Aug 29, 2012 9:56:04 AM
 */
public class HHT_1011_01 extends BusinessService implements ProcessUnit {

	public String process(Request request) {
		// TODO Auto-generated method stub
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("营业课--价格查询--商品基本信息检索, response="
				+ bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception {
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(
				configpropertyUtil.getValue("sto"));
		String sku = bean.getRequest().getParameter(
				configpropertyUtil.getValue("sku"));
		if (sku.length() > UPC_LENGTH_TWELVE) {
			sku = tranUPCtoSKU(store, sku);
		} else if (sku.length() > SKU_LENGTH
				&& sku.length() <= UPC_LENGTH_TWELVE) {
			sku = tranUPCtoSKUforLengthNinetoEleven(store, sku);
		} else if (sku.length() < IPC_LENGTH) {
			sku = fullSKU(sku);
		} else if (sku.length() == IPC_LENGTH) {// ipc转换成SKU\

			if (sku.substring(0, 1).equals("2")) {
				String sql = sqlpropertyUtil.getValue(store, "hht1011.01.04");
				List<Map> skuList = jdbcTemplateUtil.searchForList(sql,
						new Object[] { sku });
				if (skuList != null && skuList.size() > 0) {
					sku = skuList.get(0).get("hhtsku").toString();
				}
			} else {
				String sql = sqlpropertyUtil.getValue(store, "hht1011.01.00");
				List<Map> skuList = jdbcTemplateUtil.searchForList(sql,
						new Object[] { sku, sku, fullUPCTwelve(sku),
								fullUPCThirteen(sku) });
				if (skuList != null && skuList.size() > 0) {
					sku = skuList.get(0).get("hhtsku").toString();
				}
			}
		}
		String getIPCsql = sqlpropertyUtil.getValue(store, "hht1011.01.05");
		List<Map> ipcList = jdbcTemplateUtil.searchForList(getIPCsql,
				new Object[] { sku });

		String sql = sqlpropertyUtil.getValue(store, "hht1011.01.01");
		List<Map> detailList = jdbcTemplateUtil.searchForList(sql,
				new Object[] { sku, store });
		if (detailList != null && detailList.size() > 0) {

			String mssql = sqlpropertyUtil.getValue(store, "hht1011.01.06");
			List<Map> cprList = null;

			cprList = jdbcTemplateSqlServerUtil.searchForList(mssql, new Object[] { detailList.get(0).get("HHTIPC").toString() });
			/*
			if (store.equals("11108"))
			{
				log.info("11108");
				cprList = jdbcTemplateSqlServerUtil2.searchForList(mssql, new Object[] { detailList.get(0).get("HHTIPC").toString() });
			} 
			else
			{
				cprList = jdbcTemplateSqlServerUtil.searchForList(mssql, new Object[] { detailList.get(0).get("HHTIPC").toString() });
			}
			*/
			
			if (cprList != null && cprList.size() > 0) {
				detailList.get(0).put("CPR", cprList.get(0).get("CPR"));
			}

			Map subskuInfoMap = detailList.get(0);
			String setCode = subskuInfoMap.get("setCode").toString();// 获取商品子母种类（0
																		// 普通商品，
																		// 1母商品
																		// 2子商品
																		// 3母商品
																		// 4子商品）
			String parentSkuKey = "parent_sku";
			String subskusql = "";
			List<Map> subskuList = null;
			if (setCode.equals("1") || setCode.equals("3")) {
				subskuInfoMap.put(parentSkuKey, sku);
				subskusql = sqlpropertyUtil.getValue(store, "hht1011.01.02");// 获取母商品下，所有子商品的商品列表
				subskuList = jdbcTemplateUtil.searchForList(subskusql,
						new Object[] { sku });
			} else if (setCode.equals("2") || setCode.equals("4")) {// 如果是子商品，那么先获取其母商品，再获取其母商品下所有子商品
				String parentSkuSql = sqlpropertyUtil.getValue(store,
						"hht1011.01.03");
				List<Map> parentSkuList = jdbcTemplateUtil.searchForList(
						parentSkuSql, new Object[] { sku });
				if (parentSkuList.size() > 0) {
					String parentSku = parentSkuList.get(0).get(parentSkuKey)
							.toString();
					subskuInfoMap.put(parentSkuKey, parentSku);
					// 根据母商品，查询其下所有子商品
					subskusql = sqlpropertyUtil
							.getValue(store, "hht1011.01.02");// 获取母商品下，所有子商品的商品列表
					subskuList = jdbcTemplateUtil.searchForList(subskusql,
							new Object[] { parentSku });
				}
			} else {
				subskuInfoMap.put(parentSkuKey, "");
			}
			Config c = new Config("11", "Server->Client：0", String.valueOf(bean
					.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(
							configpropertyUtil.getValue("op"))));
			XmlElement[] xml = null;
			if (subskuList == null) {
				xml = new XmlElement[1];
				xml[0] = new XmlElement("info", detailList);
			} else if (subskuList.size() == 0) {
				xml = new XmlElement[1];
				xml[0] = new XmlElement("info", detailList);
				bean.getResponse().setCode(BusinessService.warncode);
				bean.getResponse().setDesc(
						configpropertyUtil.getValue("msg.1011.02"));
			} else {
				xml = new XmlElement[2];
				xml[0] = new XmlElement("info", detailList);
				xml[1] = new XmlElement("detail", subskuList);
			}
			writerFile(c, xml, bean);

		} else {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(
					configpropertyUtil.getValue("msg.1011.01"));
		}
	}

}

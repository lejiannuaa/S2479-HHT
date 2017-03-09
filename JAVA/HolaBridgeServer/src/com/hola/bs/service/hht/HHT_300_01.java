package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * 
 * @author S2138 2013-03-18
 * 
 */
public class HHT_300_01 extends BusinessService implements ProcessUnit {

	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		log.info("send response: HHT_300_01," + bean.getResponse().toString());
		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception {
		if (bean.getUser() == null || "".equals(bean.getUser())) {
			bean.getResponse().setCode("1");
			bean.getResponse().setDesc("未知的用户！");

		}
		if (bean.getUser().getStore() == null
				|| "".equals(bean.getUser().getStore())) {
			bean.getResponse().setCode("1");
			bean.getResponse().setDesc("未知的门店！");
		}
		// 门店号
		String store = bean.getUser().getStore();
		// 查询类型
		String hhttype = bean.getRequest().getParameter("hhttype");
		// 波次号
		String ggtwav = bean.getRequest().getParameter("hhtwav");
		// 收货状态
		String hhtstat = bean.getRequest().getParameter("hhtstat");

		// 查询条件不为TRF或PO提示错误
		if (hhttype == null || "".equals(hhttype)) {
			bean.getResponse().setCode("1");
			bean.getResponse().setDesc("请选择查询类型！");
		}

		// 查询的波次号为null或""
		if (ggtwav == null || "".equals(ggtwav)) {
			bean.getResponse().setCode("1");
			bean.getResponse().setDesc("请输入波次号！");
		}

		// 获取头档SQL
		String sql = null;

		// 获取明细档SQL
		StringBuffer detailSql = new StringBuffer();

		// TRF查询
		if (hhttype.equalsIgnoreCase("TRF")) {
			// 创建头档查询SQL
			sql = sqlpropertyUtil.getValue(store, "hht300.01.01");
			// 创建明细查询SQL
			detailSql.append(sqlpropertyUtil.getValue(store, "hht300.03.01"));
			// 收货状态不为null或""

			if (hhtstat.equals("0")) {

			}// 查询条件为已收货
			else if (hhtstat.equals("1")) {
				detailSql.append(" and hhtsta = '2'");
			} else {// 查询条件为未收货
				detailSql.append(" and hhtsta in ('0','1')");
			}
		} else if (hhttype.equalsIgnoreCase("PO")) { // PO查询
			// 创建查头档询SQL
			sql = sqlpropertyUtil.getValue(store, "hht300.02.01");
			// 创建明细查询SQL
			detailSql.append(sqlpropertyUtil.getValue(store, "hht300.04.01"));
			// 收货状态不为null或""
			if (hhtstat.equals("0")) {

			}
			// 查询条件为已收货
			else if (hhtstat.equals("1")) {
				detailSql.append(" and d1stat = '5'");
			} else {// 查询条件为未收货
				detailSql.append(" and d1stat in ('3','4')");
			}

		}

		// 创建查询条件的参数 波次号必填，收货状态可有可无
		Object[] o = new Object[] { ggtwav, ggtwav };

		// 查询头档信息;

		List<Map> list = jdbcTemplateUtil.searchForList(sql, o);

		Object[] b = new Object[] { ggtwav };

		// 查询明细结果
		List<Map> detailList = jdbcTemplateUtil.searchForList(detailSql
				.toString(), b);

		// 定义XML文件的头部信息
		Config c = new Config("0", "Server->Client：0", String.valueOf(bean
				.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(
						configpropertyUtil.getValue("op"))));
		// 定义XML元素的数量
		XmlElement[] xml = new XmlElement[2];
		// 定义XML元素的头档规则
		xml[0] = new XmlElement("info", list);
		// 定义XML元素的明细档规则
		xml[1] = new XmlElement("detail", detailList);
		// 根据定义的规则写入XML
		writerFile(c, xml, bean);

	}
}

package com.hola.bs.service.hht;

import java.util.ArrayList;
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
 * 收货信息检索查询
 * @author S2139
 * 2012 Jun 26, 2012 6:02:11 PM 
 */
public class HHT_203_01 extends BusinessService implements ProcessUnit {
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		String result = "";
		try {
			bean = resolveRequest(request);
			processData(bean);
//			result = processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
//		log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("send response: "+bean.getResponse().toString());
		log.info("operation hht203_01收货信息检索查询, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String store = "";
		if (bean.getUser() != null) {
			store = bean.getUser().getStore();
		} else {
			store = "13101";
		}

		String type = (String) bean.getRequest().getParameter("type");
		// String usr = (String)bean.getRequest().getParameter("usr");
		String op = (String) bean.getRequest().getParameter("op");
		String from = (String) bean.getRequest().getParameter("from");
		String to = (String) bean.getRequest().getParameter("to");
		String bcfrom = (String) bean.getRequest().getParameter("bcfrom");
		String bcto = (String) bean.getRequest().getParameter("bcto");
		String opusr = (String) bean.getRequest().getParameter("opusr");

		List<Map> detailList = new ArrayList();

		Config c = new Config("0", "Server-Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];

		// 收货查询
		if (op != null && op.equals("01")) {
			if (null != type) {
				if (type.equals("0")) {
					StringBuffer sqlString = new StringBuffer(sqlpropertyUtil.getValue(bean.getUser().getStore(),
							"hht203.01.02"));
					if (opusr != null && !opusr.equals(""))
						sqlString.append(" and crtuser='" + opusr + "' ");
					if (bcfrom != null && !bcfrom.equals(""))
					{
						while(bcfrom.length()<6)
						{
							bcfrom = "0" + bcfrom;
						}
						while(bcto.length()<6)
						{
							bcto = "0" + bcto;
						}
						sqlString.append(" and d1shnb between '" + bcfrom + "' and '" + bcto + "' ");
					}
					if (from != null && !from.equals(""))
						sqlString.append(" and crtdate between '" + from + "' and '" + to + "' ");
					System.out.println(sqlString.toString());
					detailList = jdbcTemplateUtil.searchForList(sqlString.toString(), new Object[] {});
					xml[0] = new XmlElement("detail", detailList);
				} else if (type.equals("1")) {
					StringBuffer sqlString = new StringBuffer(sqlpropertyUtil.getValue(bean.getUser().getStore(),
							"hht203.01.03"));
					if (opusr != null && !opusr.equals(""))
						sqlString.append(" and crtuser='" + opusr + "' ");
					if (bcfrom != null && !bcfrom.equals(""))
						sqlString.append(" and HHTCNO between '" + bcfrom + "' and '" + bcto + "' ");
					if (from != null && !from.equals(""))
						sqlString.append(" and crtdate between '" + from + "' and '" + to + "' ");
					System.out.println(sqlString.toString());
					detailList = jdbcTemplateUtil.searchForList(sqlString.toString(), new Object[] {});
					xml[0] = new XmlElement("detail", detailList);
				}
			} else
				throw new Exception();
		}

		// 列印选择的收货单
		if (op != null && op.equals("02")) {

		}

		writerFile(c, xml, bean);
//		return super.toXML(c, xml).replaceAll("\n", "");

	}
}

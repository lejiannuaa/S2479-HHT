package com.hola.bs.service.hht;

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
 * 
 * 实现查询接口--出货查询
 * 
 * @author Roland.Yu
 * @version 2012/03/12
 */

public class HHT_201_01 extends BusinessService implements ProcessUnit {

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
//		log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht201_01查询接口-出货查询, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String store = "";
		if (bean.getUser() != null) {
			store = bean.getUser().getStore();
		} else {
			store = "13101";
		}
		
		StringBuffer sqlString = new StringBuffer(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht201.01.02"));
		
		String bcfrom = (String) bean.getRequest().getParameter("bcfrom");
		String bcto = (String) bean.getRequest().getParameter("bcto");
		String boxbcfrom = (String) bean.getRequest().getParameter("boxbcfrom");
		String boxbcto = (String) bean.getRequest().getParameter("boxbcto");
		String from = (String) bean.getRequest().getParameter("from");
		String to = (String) bean.getRequest().getParameter("to");
		String type = (String) bean.getRequest().getParameter("type");
		String state = (String) bean.getRequest().getParameter("state");
		String frml = (String) bean.getRequest().getParameter("frml");

		if (bcfrom != null && !bcfrom.equals(""))
		{
			while(bcfrom.length()<6)
			{
				bcfrom = "0" + bcfrom;
			}
			sqlString.append(" and a.hhtno>='" + bcfrom + "'");
		}
		if (bcto != null && !bcto.equals(""))
		{
			while(bcto.length()<6)
			{
				bcto = "0" + bcto;
			}
			sqlString.append(" and a.hhtno<='" + bcto + "'");
		}
		if (boxbcfrom != null && !boxbcfrom.equals(""))
			sqlString.append(" and a.hhtcno>='" + boxbcfrom + "'");
		if (boxbcto != null && !boxbcto.equals(""))
			sqlString.append(" and a.hhtcno<='" + boxbcto + "'");
		if (from != null && !from.equals(""))
			sqlString.append(" and b.hhtcrtdt>='" + from + "'");
		if (to != null && !to.equals(""))
			sqlString.append(" and b.hhtcrtdt<='" + to + "'");
		if (type != null && !type.equals("")) {
			if (type.equals("0"))
				sqlString.append(" and a.hhttyp='RTV'");
			else if (type.equals("1"))
				sqlString.append(" and a.hhttyp='TRF'");
		}
		if (state != null && !state.equals(""))
			sqlString.append(" and a.hhtsuc='" + state + "'");
		if (frml != null && !frml.equals("")) {
			if (frml.equals("0"))
				sqlString.append(" and b.hhtesy='HHT'");
			else if (frml.equals("1"))
				sqlString.append(" and b.hhtesy='JDA'");
		}
		// System.out.println(sqlString);

		List<Map> list = jdbcTemplateUtil.searchForList(sqlString.toString(), new Object[] {});
		
		log.info("成功失败条目数："+list.size());
		for(int i=0;i<list.size();i++)
		{
			log.info("第"+i+"条状态为："+list.get(i).get("状态").toString());
			if(list.get(i).get("状态").toString().equals("Y"))
			{
				log.info("查询是否有历史失败记录");
				String hhtcno = list.get(i).get("箱号").toString();
				for(int j=0;j<list.size();j++)
				{
					if((list.get(j).get("箱号").toString().equals(hhtcno))&&(list.get(j).get("状态").toString().equals("N")))
					{
						log.info("删除历史失败记录");
						String[] deleteSqls = new String[2];
						Object[] args = new Object[2];
						deleteSqls[0]=sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht201.01.03");
						args[0]=new Object[] {hhtcno};
						deleteSqls[1]=sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht201.01.04");
						args[1]=new Object[] {hhtcno};
						jdbcTemplateUtil.update(deleteSqls,args);
						break;
					}
				}
			}
		}
		
		list = jdbcTemplateUtil.searchForList(sqlString.toString(), new Object[] {});
		
		Config c = new Config("0", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		xml[0] = new XmlElement("detail", list);

		writerFile(c, xml, bean);

	}

}

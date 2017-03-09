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

public class HHT_111_01 extends BusinessService implements ProcessUnit{

	public String process(Request request) {
		// TODO Auto-generated method stub
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			process(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht111.01预约查询--商品检索, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void process(BusinessBean bean) throws Exception {
		// TODO Auto-generated method stub
		String store = bean.getUser().getStore();
		String hhtcno = bean.getRequest().getParameter(configpropertyUtil.getValue("hhtcno"));
		
		String querysql1 = sqlpropertyUtil.getValue(store,"hht111.01.01");
		List<Map> list=jdbcTemplateUtil.searchForList(querysql1,new Object[]{hhtcno,hhtcno,hhtcno,hhtcno});
				
		if(list == null && list.size()==0){
			String querysql2 = sqlpropertyUtil.getValue(store,"hht111.01.02");
		    list=jdbcTemplateUtil.searchForList(querysql2,new Object[]{hhtcno,hhtcno});
		}
		
		if(list == null && list.size()==0){
			String querysql3 = sqlpropertyUtil.getValue(store,"hht111.01.03");
		    list=jdbcTemplateUtil.searchForList(querysql3,new Object[]{hhtcno});
		}
		
		if(list != null && list.size()>0)
		{
			if(list.get(0).get("hhtsts").equals("已预约"))
			{
				Config c=new Config("0", "Server->Client：0",String.valueOf(bean.getRequest().getParameter("request"))+String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
				XmlElement[] xml=new XmlElement[1];
				xml[0]=new XmlElement("detail", list);
				writerFile(c, xml,bean);
			}
			else if(list.get(0).get("hhtsts").equals("实物出货"))
			{
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("该箱号已完成实物出货");
			}
		}
		else
		{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("该箱号尚未预约");
		}
	}

}

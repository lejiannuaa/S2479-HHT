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

public class HHT_108_01 extends BusinessService implements ProcessUnit{

	public String process(Request request) {
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
		log.info("operation hht108.01正常品预约--商品检索, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void process(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getUser().getStore();
		String hhtcno = bean.getRequest().getParameter(configpropertyUtil.getValue("hhtcno"));
		
		String querysql = sqlpropertyUtil.getValue(store,"hht108.02.01");
		List<Map> list=jdbcTemplateUtil.searchForList(querysql,new Object[]{hhtcno,hhtcno});
		
		if(list!=null&&list.size()>0){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.108.02"));
		}else{
			
		
		
		String querysql1 = sqlpropertyUtil.getValue(store,"hht108.01.01");
		list=jdbcTemplateUtil.searchForList(querysql1,new Object[]{hhtcno,hhtcno});
		
		if(list == null && list.size()==0){
			String querysql2 = sqlpropertyUtil.getValue(store,"hht108.01.02");
		    list=jdbcTemplateUtil.searchForList(querysql2,new Object[]{hhtcno});
		}
		
		if(list != null && list.size()>0){
			
			Config c=new Config("0", "Server->Client：0",String.valueOf(bean.getRequest().getParameter("request"))+String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			XmlElement[] xml=new XmlElement[1];
			xml[0]=new XmlElement("detail", list);
			writerFile(c, xml,bean);
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(
					configpropertyUtil.getValue("msg.108.01"));
		}
		}
	}

}

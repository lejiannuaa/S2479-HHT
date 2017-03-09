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
 * 店店调拨修改界面按箱号检索调拨信息商品检索
 * @author s1713 modify s2139
 *
 */
public class HHT_104_01  extends BusinessService implements ProcessUnit {
	


	public String process(Request request) {
		BusinessBean bean=new BusinessBean();
		try {
			 bean=resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。"+e.getMessage());
			log.error("", e);
		}
		return  bean.getResponse().toString();
	}
	
	
	

	private void processData(BusinessBean bean) throws Exception{
		String store="";
		if(bean.getUser()!=null){
			store=bean.getUser().getStore();
		}else{
			store="13101";
		}
		
		List<Map> list=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht104.01.01"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc"))});
		if(list!=null && list.size()>0){
			String state = list.get(0).get("state").toString();
			
			if(!state.equalsIgnoreCase("1N")){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.101.01"));
			}else{
				List<Map> list2=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht104.01.02"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc"))});
				Config c=new Config("0", "Server->Client：0",String.valueOf(bean.getRequest().getParameter("request"))+String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
				XmlElement[] xml = new XmlElement[2];
				xml[0] = new XmlElement("info", list);
				xml[1] = new XmlElement("detail", list2);
				writerFile(c, xml, bean);
			}
		}else{
			Config c=new Config("0", "Server->Client：0",String.valueOf(bean.getRequest().getParameter("request"))+String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			writerFile(c, null, bean);
		}

	}
}

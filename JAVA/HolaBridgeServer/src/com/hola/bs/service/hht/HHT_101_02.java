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
 * 出货到厂商，厂商信息检索
 * @author s1713 modify s2139
 *
 */
public class HHT_101_02  extends BusinessService implements ProcessUnit {
	


	public String process(Request request) {
		
		BusinessBean bean=new BusinessBean();
		try {
			 bean=resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。"+e.getMessage());
			log.error("",e);
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht101.02出货到厂商，厂商信息检索, response="+bean.getResponse().toString());
			// System.out.println("001_01:response="+response.toString());
		return  bean.getResponse().toString();
	}
	
	private void processData(BusinessBean bean) throws Exception{
			List<Map> list=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht101.02.01"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc"))});
			if(list!=null&&list.size()>0){
				//20120420 以下逻辑暂时取消 by mark
//				if(!"N".equals(list.get(0).get("HHTVF1"))){//是否可退厂商 需要修改为字段名称
				if(list.get(0).get("HHTVTP").toString().equals("1") && list.get(0).get("HHTVF4").toString().equals("Y")){
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc("错误的厂商，该厂商已停权");
				}

	//			else if(list.get(0).get("HHTVF1").toString().equals("1")){
		//			bean.getResponse().setCode(BusinessService.errorcode);
			//		bean.getResponse().setDesc("该厂商不允许退货");
				
			//	}
				
				else{
					Map dataMap = list.get(0);
					if(dataMap.get("HHTVF4") == null || dataMap.get("HHTVF4").toString().length()==0){
						list.get(0).put("HHTVF4", "N");
					}
					Config c=new Config("0", "Server->Client：0",String.valueOf(bean.getRequest().getParameter("request"))+String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
					XmlElement[] xml=new XmlElement[1];
					xml[0]=new XmlElement("info", list);
					
					if(list!=null && list.size()>0)
						bean.getUser().setAttribute("HHTVTP", String.valueOf(list.get(0).get("HHTVTP")));
					
					writerFile(c, xml,bean);
				}
					
//				}else{
//					bean.getResponse().setCode(BusinessService.errorcode);
//					bean.getResponse().setDesc("该厂商为不可退厂商！");
//				}
			}else{
//				bean.getResponse().setCode(BusinessService.errorcode);
//				bean.getResponse().setDesc("厂商不存在！");
				
				Config c=new Config("0", "Server->Client：0",String.valueOf(bean.getRequest().getParameter("request"))+String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
				XmlElement[] xml=new XmlElement[1];
				xml[0]=new XmlElement("info", list);
				
				writerFile(c, xml,bean);
			}
	}
}

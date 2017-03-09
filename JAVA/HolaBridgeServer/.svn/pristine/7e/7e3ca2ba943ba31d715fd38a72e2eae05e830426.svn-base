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
 * 出货到厂商的商品信息检索
 * @author s1713 modify s2139
 *
 */
public class HHT_101_01  extends BusinessService implements ProcessUnit {
	


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
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht101.01出货到厂商的商品信息检索, response="+bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		return  bean.getResponse().toString();
	}
	
	
	

	private void processData(BusinessBean bean) throws Exception{
			String store="";
			if(bean.getUser()==null){
				store="13101";
			}else{
				store=bean.getUser().getStore();
				if(store==null||"".equals(store)){//测试用
					store="13101";
				}
			}
			List<Map> list=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht101.01.02"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc")),store});
			if(list!=null && list.size()>0){
				String state = list.get(0).get("state").toString();
			//	if(state.equals("10") || state.equals("1Y")){//已提交或申请状态不能再次提交  !='1' 1可以修改
				if(!state.equals("1")){	
					int stateNo = Integer.parseInt(state);
					bean.getResponse().setCode(BusinessService.errorcode);
					if(stateNo == 0){
						bean.getResponse().setDesc(configpropertyUtil.getValue("msg.101.01"));
					}else if(stateNo>1){
						bean.getResponse().setDesc(configpropertyUtil.getValue("msg.101.02"));
					}
				}else{
					List list2=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht101.01.01"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc")),store});
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

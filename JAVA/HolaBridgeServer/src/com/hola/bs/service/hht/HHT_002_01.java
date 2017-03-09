package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * 檢索调拨单收货商品信息
 * @author s1713 modify s2139
 *
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_002_01  extends BusinessService implements ProcessUnit {
	


	public String process(Request request) {
		BusinessBean bean=new BusinessBean();
		try {
			 bean=resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。"+e.getMessage());
			log.error("", e);
			throw new RuntimeException();
		}
//		log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht002.01 檢索调拨单收货商品信息, response="+bean.getResponse().toString());

		// System.out.println("001_01:response="+response.toString());
		return  bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception{
		
			List<Map> statusMapList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht002.01.02"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc"))});
			if(statusMapList==null || statusMapList.size()==0){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("不存在的调拨箱号。");
				return;
			}else{
				Map m = statusMapList.get(0);
				String status = m.get("state").toString();
				if(status.equals("0")){//状态为0 改成1
					String sql[]=new String[]{ sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht001.05.01")};
					String username="";
					if(bean.getUser()!=null){
						username=bean.getUser().getName();
					}else{
						throw new Exception("当前用户已登出，或系统异常，找不到操作用户");
					}
					Object o[]=new Object[1];
					o[0]=new Object[]{username,String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("bc")))};
					jdbcTemplateUtil.update(sql, o);
				}else if(status.equals("1")){//状态为1，提示有用户在收货中
					String crtusr = m.get("CRTUSER").toString();
					if(!bean.getUser().getName().equals(crtusr)){
						bean.getResponse().setCode(BusinessService.warncode);
						bean.getResponse().setDesc("当前该箱由用户："+crtusr+"在执行收货中。");
					}
					
				}else if(status.equals("2")){
					
				}else{
					
				}
				
				List<Map> list=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht002.01.01"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc"))});
				
				List<Map> list_ = new ArrayList<Map>();
				
				for(int i=0;i<list.size();i++){
					//原始的对象
					Map om = list.get(i);
					//新的对象
					Map am = skuExist(list_ , om.get("HHTSKU").toString(), "HHTSKU");
					//如果新的对象已经存在，则修改对象，如果不存在，则新增对象
					if(am!=null){
						am.put("UPC", om.get("UPC")+","+am.get("UPC"));
					}else{
						list_.add(om);
					}
				}
				Config c=new Config("0", "Server->Client：0",String.valueOf(bean.getRequest().getParameter("request"))+String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
				XmlElement[] xml=new XmlElement[2];
				xml[0]=new XmlElement("info",statusMapList);
				xml[1]=new XmlElement("detail", list_);
				
				writerFile(c, xml,bean);
			}
	}
}

package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.bean.JsonBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.JsonUtil;

/**
 * 实物出货保存操作
 * @author S2139
 * 2012 Sep 10, 2012 1:53:34 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_202_02 extends BusinessService implements ProcessUnit{
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
			throw new RuntimeException();
		}
//		log.info("response=" + bean.getResponse().toString());
		
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht202_02, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String store = bean.getUser()!=null?bean.getUser().getStore():"13101";
		String bc = String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("bc")));
		String state = bean.getRequest().getParameter(configpropertyUtil.getValue("state"));
		String hhtesy = bean.getRequest().getParameter(configpropertyUtil.getValue("hhtesy"));
		if(state != null && state.length()>0){
			if(state.equalsIgnoreCase("3Y")){
				List nodeNameList = new ArrayList();
				nodeNameList.add("detail");
				JsonBean json=JsonUtil.jsonToList(String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("xml"))), nodeNameList);		
				Map data[] = json.getData().get("detail");
				String sku = null;
				int reallycount = 0;
				String sql[] = new String[data.length+1];
				Object o[] = new Object[data.length+1];
				
//				String boxbc = bean.getRequest().getParameter("boxbc"); 
				
				String sysId = bean.getUser()!=null?bean.getUser().getStore()+this.systemUtil.getSysid():this.systemUtil.getSysid();
				int i=0;
				for(Map datamap : data){
					sku = String.valueOf(datamap.get("SKU"));
					if (datamap.get("Realoutputs") != null)
							reallycount = Integer.parseInt(datamap.get("Realoutputs").toString());
					else throw new Exception();
					sql[i] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht202.02.01");
					o[i] = new Object[] { sysId, sku, bc, hhtesy,bc, bc};
					i++;
				}
				sql[i] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht202.02.02");
				o[i] = new Object[]{sysId,"4",bc,hhtesy};

				String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
				String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
				super.update(bean.getRequest().getRequestID(), sysId, sql, o ,bean.getUser().getStore(),guid,requestValue,"");

			}else{
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.202.02.02"));
			}
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.202.02.01"));
		}
	}
	
}

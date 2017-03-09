package com.hola.bs.service.hht;


import java.util.Date;
import java.util.List;
import java.util.Map;




import org.apache.log4j.MDC;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.JdbcTemplateUtil;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.ReserveDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;


public class HHT_111_02 extends BusinessService implements ProcessUnit{

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
		log.info("operation hht111.02正常品出货--提交, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
		
	}

	private void process(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		
		String store = bean.getUser().getStore();
		
		String today = DateUtils.date2StringDate(new Date());
	
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		ReserveDetail[] tReserveDetaillist = JsonUtil.getDetail(jsonObject,ReserveDetail.class);
		String sysId = "";
		if (bean.getUser() != null) {
			sysId = store + this.systemUtil.getSysid();
		} else {
			sysId = this.systemUtil.getSysid();
		}
		int no = 0;
		String[] batchSqls = new String[tReserveDetaillist.length];
		Object[] o = new Object[tReserveDetaillist.length];
		
		for(ReserveDetail tReserveDetail : tReserveDetaillist){
			
			String checksql = sqlpropertyUtil.getValue(store,"hht111.02.04");
			List<Map> checklist = jdbcTemplateUtil.searchForList(checksql,new Object[]{tReserveDetail.getHhtcno(),tReserveDetail.getHhtcno()});
			if(checklist!=null&&checklist.size()>0){
			
				Date now = new Date();
		
				String stime = checklist.get(0).get("SRCDAT").toString()+checklist.get(0).get("SRCTIM").toString();
			
				Date d = DateUtils.getTime(stime);				
				
				if((now.getTime()-d.getTime())<=300000){
					
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc("箱号/单号:"+ tReserveDetail.getHhtcno()+"不能出货，预约5分钟后才能下单出货");
					break;
				}
				else
				{
					String querysql1 = sqlpropertyUtil.getValue(store,"hht111.02.00");
					List<Map> list=jdbcTemplateUtil.searchForList(querysql1,new Object[]{tReserveDetail.getHhtcno(),tReserveDetail.getHhtcno()});
					
					if(list==null&&list.size()==0){
						
						if(tReserveDetail.getHhtcno().length()>6){
							batchSqls[no] = sqlpropertyUtil.getValue(store, "hht111.02.02");
							o[no] = new Object[]{tReserveDetail.getHhtcno(),today,bean.getUser().getName(),today,sysId};
							no++;
						}else{
							batchSqls[no] = sqlpropertyUtil.getValue(store, "hht111.02.03");
							o[no] = new Object[]{tReserveDetail.getHhtcno(),today,bean.getUser().getName(),today,sysId};
							no++;
						}
						
					}else{
						batchSqls[no] = sqlpropertyUtil.getValue(store, "hht111.02.01");
						o[no] = new Object[]{today,sysId,tReserveDetail.getHhtcno(),tReserveDetail.getHhtcno()};
						no++;
					}

				}
			}
		}
		
		if(bean.getResponse().getCode().equals(BusinessService.successcode))
		{
			String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
			String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
			super.update(bean.getRequest().getRequestID(), sysId, batchSqls, o, store,guid,requestValue,sysId);
			//jdbcTemplateUtil.update(batchSqls, o);
		}
		
	}	
	

	
}

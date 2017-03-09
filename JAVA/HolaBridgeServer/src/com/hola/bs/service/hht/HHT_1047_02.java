package com.hola.bs.service.hht;

import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.HtoB2f;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate10;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

public class HHT_1047_02 extends BusinessService implements ProcessUnit{
	
	
	public String process(Request request) {
		// TODO Auto-generated method stub
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
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("库存调整新增-提交, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}
	
	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String maintain = bean.getRequest().getParameter(configpropertyUtil.getValue("maintain"));
		String today = DateUtils.date2String(new Date());
	
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		HtoB2f[] tHtoB2f = JsonUtil.getDetail(jsonObject,HtoB2f.class);
		String sysId = "";
		if (bean.getUser() != null) {
			sysId = store + this.systemUtil.getSysid();
		} else {
			sysId = this.systemUtil.getSysid();
		}
		
		if(maintain.equals("True")){
			String startTime = bean.getRequest().getParameter(configpropertyUtil.getValue("starttime"));
			jdbcTemplateUtil.update(new String[]{sqlpropertyUtil.getValue(store, "hht1047.02.04")}, new Object[]{new Object[]{today,bean.getUser().getName()}});
			
		}
		String time = DateUtils.string2TotalTime(new Date());
		if(tHtoB2f!=null){
			int no = 0;
			String[] batchSqls = new String[tHtoB2f.length];
			Object[] o = new Object[tHtoB2f.length];
			for(HtoB2f htoB2fDto:tHtoB2f){				
				
				batchSqls[no] = sqlpropertyUtil.getValue(store, "hht1047.02.01");
				o[no] = new Object[]{store,htoB2fDto.getSku(),htoB2fDto.getInv_adj_no(),htoB2fDto.getInv_act_no(),htoB2fDto.getAdj_reason(),bean.getUser().getName(),time,"Y",sysId};
				no++;
			}
			jdbcTemplateUtil.update(batchSqls, o);
			
			
			
			
			
		}
		
		
		
	}

}

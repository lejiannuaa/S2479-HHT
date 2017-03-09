package com.hola.bs.service.hht;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.ReserveDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.JsonUtil;

public class HHT_110_02 extends BusinessService implements ProcessUnit{

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
		log.info("operation hht110.02预约查询--取消预约, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void process(BusinessBean bean) {
		// TODO Auto-generated method stub
		
		String store = bean.getUser().getStore();
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
			
			batchSqls[no] = sqlpropertyUtil.getValue(store, "hht110.02.01");
			o[no] = new Object[]{tReserveDetail.getHhtcno(),tReserveDetail.getHhtcno()};
			no++;
		}
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		super.update(bean.getRequest().getRequestID(), sysId, batchSqls, o, store,guid,requestValue,sysId);
		//jdbcTemplateUtil.update(batchSqls, o);
	
	}

}

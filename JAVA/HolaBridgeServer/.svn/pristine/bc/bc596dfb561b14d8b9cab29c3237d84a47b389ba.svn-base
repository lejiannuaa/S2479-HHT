package com.hola.bs.service.hht;

import java.util.Date;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.ReserveDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

public class HHT_112_02 extends BusinessService implements ProcessUnit{

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
		log.info("operation hht109.02其他品出货--出货, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void process(BusinessBean bean) {
		// TODO Auto-generated method stub

		String store = bean.getUser().getStore();
		
		String today = DateUtils.date2StringDate(new Date());
		String sysId = "";
		if (bean.getUser() != null) {
			sysId = store + this.systemUtil.getSysid();
		} else {
			sysId = this.systemUtil.getSysid();
		}
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		ReserveDetail[] tReserveDetaillist = JsonUtil.getDetail(jsonObject,ReserveDetail.class);
		
		int no = 0;
		String[] batchSqls = new String[tReserveDetaillist.length];
		Object[] o = new Object[tReserveDetaillist.length];
		
		for(ReserveDetail tReserveDetail : tReserveDetaillist){
			batchSqls[no] = sqlpropertyUtil.getValue(store, "hht112.02.01");
			o[no] = new Object[]{tReserveDetail.getHhtvol(),tReserveDetail.getHhtnum(),today,sysId,tReserveDetail.getHhtcno()};
			no++;	
		}
		
		String[] insertSqls = new String[4];
		insertSqls[0]=sqlpropertyUtil.getValue(store, "hht112.02.03");
		insertSqls[1]=sqlpropertyUtil.getValue(store, "hht112.02.04");
		insertSqls[2]=sqlpropertyUtil.getValue(store, "hht112.02.05");
		insertSqls[3]=sqlpropertyUtil.getValue(store, "hht112.02.06");
		
		Object[] oo = new Object[4];
		oo[0] = new Object[]{store,today,bean.getUser().getName(),today};
		oo[1] = new Object[]{store,today,bean.getUser().getName(),today};
		oo[2] = new Object[]{store,today,bean.getUser().getName(),today};
		oo[3] = new Object[]{store,today,bean.getUser().getName(),today};
		
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		super.update(bean.getRequest().getRequestID(), sysId, batchSqls, o, store,guid,requestValue,sysId);
		jdbcTemplateUtil.update(insertSqls, oo);
	}

}

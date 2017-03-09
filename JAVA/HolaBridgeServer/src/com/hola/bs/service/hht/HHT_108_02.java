package com.hola.bs.service.hht;

import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.ReserveDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

public class HHT_108_02 extends BusinessService implements ProcessUnit{

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
		log.info("operation hht108.02正常品预约--预约, response="+bean.getResponse().toString());

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
			
			String querysql = sqlpropertyUtil.getValue(store,"hht108.02.01");
			List<Map> list=jdbcTemplateUtil.searchForList(querysql,new Object[]{tReserveDetail.getHhtcno(),tReserveDetail.getHhtcno()});
			
			if(list!=null&&list.size()>0){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.108.02"));
			}else{
				String detailsql = sqlpropertyUtil.getValue(store,"hht108.02.00");
				List<Map> detailList = jdbcTemplateUtil.searchForList(detailsql,new Object[]{tReserveDetail.getHhtcno(),tReserveDetail.getHhtcno()});
				String strHHTTYP = null;
				String strHHTHNO = null;
				String strHHTNO= null;
				String strHHTCNO= null;
				String strHHTFLC= null;
				String strHHTTLC= null;
				
				if(detailList!=null&&detailList.size()>0){
					strHHTTYP = detailList.get(0).get("HHTTYP").toString();
					strHHTHNO = detailList.get(0).get("HHTHNO").toString();
					if(detailList.get(0).get("HHTNO")!=null&&detailList.get(0).get("HHTNO").toString().length()>0){
						strHHTNO = detailList.get(0).get("HHTNO").toString();
					}
					if(detailList.get(0).get("HHTCNO")!=null&&detailList.get(0).get("HHTCNO").toString().length()>0){
						strHHTCNO = detailList.get(0).get("HHTCNO").toString();
					}
					
					strHHTFLC = detailList.get(0).get("HHTFLC").toString();
					strHHTTLC = detailList.get(0).get("HHTTLC").toString();
				}
				
				
					batchSqls[no] = sqlpropertyUtil.getValue(store, "hht108.02.02");
					o[no] = new Object[]{strHHTTYP,strHHTHNO,strHHTFLC,strHHTTLC,strHHTNO,strHHTCNO,tReserveDetail.getHhtvol(),tReserveDetail.getHhtwei(),today,bean.getUser().getName(),today,sysId};
					no++;
				
			}
			
			
			
			
		}
		
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		super.update(bean.getRequest().getRequestID(), sysId, batchSqls, o, store,guid,requestValue,sysId);
		//jdbcTemplateUtil.update(batchSqls, o);
		
	}

}

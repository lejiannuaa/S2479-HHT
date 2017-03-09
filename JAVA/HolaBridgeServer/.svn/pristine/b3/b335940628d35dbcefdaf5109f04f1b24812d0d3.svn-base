package com.hola.bs.service.hht;

import java.util.Date;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.SecondCountDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * 保存复盘数据信息，上传复盘数据信息，修改复盘柜号状态
 * @author S2139
 * 2012 Aug 27, 2012 3:10:32 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_601_02 extends BusinessService implements ProcessUnit {

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
		MDC.put("stoNo", bean.getRequest().getParameter(configpropertyUtil.getValue("sto")));
		log.info("保存复盘数据信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean)throws Exception {
		// TODO Auto-generated method stub
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String stk_second_type = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_second_type"));
		String date = DateUtils.date2String2(new Date());
		
		String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));

		
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		SecondCountDetail[] scds = JsonUtil.getDetail(jsonObject, SecondCountDetail.class);
		
		String[] sqls = new String[scds.length+2];
		Object[] o = new Object[scds.length+2];
		int no = 0;
		for(SecondCountDetail scd:scds){
			sqls[no] = sqlpropertyUtil.getValue(store, "hht601.01.02");
			o[no] = new Object[]{scd.getSku_second_qty(),scd.getAdj_reason(),userId,date,sysId,stk_no,loc_no,stk_second_type,scd.getSku(),scd.getSel_no()};
			no++;
		}
		
		sqls[no] = sqlpropertyUtil.getValue(store, "hht601.01.03");
		o[no] = new Object[]{userId,date,stk_no,loc_no,userId,stk_second_type};
		
		//增加update stk_stkplan_set set STK_STATUS='E' where STK_NO=? and STO_NO=?
		sqls[no+1] = sqlpropertyUtil.getValue(store, "hht601.01.04");
		o[no+1] = new Object[]{stk_no,store};
		
		super.update(bean.getRequest().getRequestID(), sysId, sqls, o,bean.getUser().getStore(),guid,requestValue,"" );

//		int ret = jdbcTemplateUtil.update(sqls, o);
//		if(ret >=1){
//			bean.getResponse().setCode(BusinessService.successcode);
//			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.501.01"));
//			
//			//call remote method
//		}
	}

}

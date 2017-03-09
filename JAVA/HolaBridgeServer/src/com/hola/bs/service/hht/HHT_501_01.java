package com.hola.bs.service.hht;

import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.FirstCountDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * 保存初盘数据
 * @author S2139
 * 2012 Aug 23, 2012 5:55:22 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_501_01 extends BusinessService implements ProcessUnit{

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
			throw new RuntimeException(e);
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter(configpropertyUtil.getValue("sto")));
		log.info("保存初盘数据, response="+bean.getResponse().toString());
		System.out.println("返回的结果是：===================》"+bean.getResponse().toString());
		return bean.getResponse().toString();
	} 
	
	public void processData(BusinessBean bean) throws Exception{
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String sto = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String date = DateUtils.date2String2(new Date());
		
		String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));

		String checkSql = sqlpropertyUtil.getValue(sto,"hht501.01.04");
		Object[] check = new Object[]{ stk_no,loc_no,userId,sto };
		List<Map> checkList = jdbcTemplateUtil.searchForList(checkSql,check);

		if(checkList==null||checkList.size()==0){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.501.03"));
		}else{
			String str = "{\"root\":{\"config\":{\"type\":\"5\",\"direction\":\"Client->Server\",\"id\":\"50102\"},\"detail\":[{\"sel_no\":\"1\",\"sku\":\"000000254\",\"sku_dsc\":\"黄瓜香玻璃罐蜡14oz\",\"sku_first_qty\":\"12\"}]}}";
//			com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(str);
			com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
			FirstCountDetail[] detail = JsonUtil.getDetail(jsonObject, FirstCountDetail.class);
			
			String[] sqls = new String[detail.length];
			int i=0;
			Object[] o = new Object[detail.length];
			for(FirstCountDetail f : detail){
				sqls[i] = sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht501.01.01");
				o[i] = new Object[] {stk_no,sto,loc_no,f.getSel_no(),f.getSku(),f.getSku_dsc(),f.getSku_first_qty(),userId,date,userId,date,sysId};
				i++;
			}
			//修改初盘柜号状态
			String locUpdateSql = sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht501.01.02");
			Object oLoc = new Object[]{stk_no,loc_no,userId};
			
			String[] finalSqls = Arrays.copyOf(sqls, sqls.length+1);
			finalSqls[finalSqls.length-1] = locUpdateSql;
			
			Object[] finalOParams = Arrays.copyOf(o, o.length+1);
			finalOParams[finalOParams.length-1] = oLoc;
//			Thread.sleep(5000);
			try{
				System.out.println("update start at "+new Date());
				super.update(bean.getRequest().getRequestID(), sysId, finalSqls, finalOParams,bean.getUser().getStore(),guid,requestValue,"" );
				System.out.println("update end at "+new Date());
			} catch (Exception e) {
				System.out.println(e.getMessage());
			}
			System.out.println("501.01 update complete.");
//			int ret = jdbcTemplateUtil.update(finalSqls, finalOParams);
//			if(ret == finalSqls.length){//本地修改成功，数据上传到远程
//			bean.getResponse().setCode(BusinessService.successcode);
//			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.501.01"));
//			System.out.println("501.01 :"+BusinessService.successcode+". msg: "+configpropertyUtil.getValue("msg.501.01"));	
////				//to call remote method
//			}
		}
		
//		
	}
	
}

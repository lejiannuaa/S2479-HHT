package com.hola.bs.service.hht;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 跳转到复盘明细界面前的盘点状态及柜号状态的变更
 * @author S2139
 * 2012 Aug 24, 2012 5:07:36 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_600_03 extends BusinessService implements ProcessUnit {

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
		log.info("进入复盘明细界面前，验证柜号及单号状态, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String stk_type = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_second_type"));
		
		String getLocStatusSql = sqlpropertyUtil.getValue(store, "hht600.01.03");
		Object[] o = new Object[]{stk_no,userId,loc_no,stk_type};
		List<Map<String,String>> statusList = jdbcTemplateUtil.searchForList(getLocStatusSql, o);
		String status = statusList.get(0).get("stk_loc_status");
		if(status.equals("1")){//复盘柜号状态为已指派
			String getStkStatusSql = sqlpropertyUtil.getValue(store, "hht600.01.04");
			Object[] oStk = new Object[]{stk_no};
			boolean flag = false;
			List<Map<String,String>> stkStatusList = jdbcTemplateUtil.searchForList(getStkStatusSql, oStk);
			String stkStatus = stkStatusList.get(0).get("stk_status");
			if(stkStatus.equals("D")){//复盘的盘点单状态为复盘未开始
				flag = true;
			}
			String[] updateSqls = new String[1];
			Object[] oParams = new Object[1];
			updateSqls[0] = sqlpropertyUtil.getValue(store, "hht600.01.05");//改变柜号状态
			oParams[0] = o;
			
			String[] finalUpdateSqls = null;
			Object[] finalObjectParams = null;
			if(flag){
				String updateStkStatus = sqlpropertyUtil.getValue(store, "hht600.01.06");//改变盘点单状态
				
				finalUpdateSqls = Arrays.copyOf(updateSqls, updateSqls.length+1);
				finalUpdateSqls[finalUpdateSqls.length-1] = updateStkStatus;
				
				finalObjectParams = Arrays.copyOf(oParams, oParams.length+1);
				finalObjectParams[finalObjectParams.length-1] = oStk;
			}else{
				finalUpdateSqls = updateSqls;
				finalObjectParams = oParams;
				
			}
			
			int ret = jdbcTemplateUtil.update(finalUpdateSqls, finalObjectParams);
			//CALL 远程方法
		}
		
	}

}

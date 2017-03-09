package com.hola.bs.service.hht;

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
 * 进入初盘录入时所需的柜号验证
 * @author S2139
 * 2012 Aug 23, 2012 3:18:53 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_500_03 extends BusinessService implements ProcessUnit {

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
		log.info("进入初盘明细前，验证柜号状态信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception{
		String userId = bean.getUser() != null?bean.getUser().getName():"s2139";
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		Object[] o = new Object[]{stk_no,userId,loc_no};
//		o[0] = stk_no;
//		o[1] = userId;
//		o[2] = loc_no;//盘点柜号
		List<Map<String,String>> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,"hht500.00.03"), o);
		if(list!=null&&list.size()>0){
			String loc_status = list.get(0).get("loc_status");
			if(loc_status.equals("O")){//已指派
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("该柜号已完成盘点");
				/*
//				String sql = sqlpropertyUtil.getValue(store,"hht500.00.04");
//				jdbcTemplateUtil.update(new String[]{sql}, o);
				String updateSql[] = null;
				Object[] oParam = null;
				//检查盘点单号的状态是否为 盘点未开始
				boolean updateStkFlag = false;
				List<Map> stkStatusList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,"hht500.00.05"), new Object[]{o[0]});
				
				String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
				String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
				String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
				
				if(stkStatusList!=null&&list.size()>0){
					updateStkFlag = true;
					updateSql = new String[2];
					oParam = new Object[2];
					updateSql[0] = sqlpropertyUtil.getValue(store,"hht500.00.04");
					oParam[0] = o;
					updateSql[1] = sqlpropertyUtil.getValue(store,"hht500.00.06");
					oParam[1] = new Object[]{stk_no};
				}else{
					updateSql = new String[1];
					oParam = new Object[1];
					updateSql[0] = sqlpropertyUtil.getValue(store,"hht500.00.04");
					oParam[0] = o;
				}
				jdbcTemplateUtil.update(updateSql, oParam);
//				super.update(bean.getRequest().getRequestID(), sysId, updateSql, oParam,bean.getUser().getStore(),guid,requestValue,"" );
				// 这里调用远程服务 
//				if(updateStkFlag){
					//远程需要修改盘点单状态为“盘点中”
//				}
				//这里远程修改柜号状态
				 */
			}
		}
	}
}

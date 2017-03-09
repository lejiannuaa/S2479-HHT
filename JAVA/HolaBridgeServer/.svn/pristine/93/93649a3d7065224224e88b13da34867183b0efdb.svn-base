package com.hola.bs.service.hht;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.GroupInfo;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.JsonUtil;

/**
 * 保存店铺端架的基本信息
 * @author S2139
 * 2012 Aug 30, 2012 3:48:28 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_1042_02 extends BusinessService implements ProcessUnit {

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
			throw new RuntimeException(e.getMessage());
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("保存端架基本信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String groupId = bean.getRequest().getParameter(configpropertyUtil.getValue("groupId"));
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		GroupInfo[] gis = JsonUtil.getDetail(jsonObject, GroupInfo.class);
		String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
//		String opType = bean.getRequest().getParameter(configpropertyUtil.getValue("opType"));
		String sql = null;
		Object[] o = null;
//		if(opType.equals("1")){//新增标识
		sql = sqlpropertyUtil.getValue(store, "hht1042.01.01");
		o = new Object[]{groupId,gis[0].getGroup_desc(),store,gis[0].getStart_valid_date().replaceAll("-", ""),gis[0].getEnd_valid_date().replaceAll("-", ""),null};
//		}else if(opType.equals("2")){//修改标识
//			sql = sqlpropertyUtil.getValue(store, "hht1042.01.01");
//			o = new Object[]{gis[0].getStart_valid_date(),gis[0].getEnd_valid_date(),store,groupId};
//		}
//		jdbcTemplateUtil.update(new String[]{sql}, new Object[]{o});
		//String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		// requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		
		jdbcTemplateUtil.update(new String[]{sql}, new Object[]{o});
		//super.update(bean.getRequest().getRequestID(), sysId, new String[]{sql}, new Object[]{o}, store,guid,requestValue,sysId);
	}

}

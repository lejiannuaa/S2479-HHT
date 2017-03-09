package com.hola.bs.service.hht;

import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.SkuPrtDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * 上传新增价签信息 需要增加操作，上传至HHTSERVER
 * @author S2139
 * 2012 Aug 30, 2012 10:11:29 AM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_1034_02 extends BusinessService implements ProcessUnit {

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
		log.info("保存并上传价签信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String date = DateUtils.date2String2(new Date());
		
		String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		SkuPrtDetail[] sdps = JsonUtil.getDetail(jsonObject, SkuPrtDetail.class);
		String[] sqls = new String[sdps.length];
		Object[] o = new Object[sdps.length];
		int no = 0;
		for(SkuPrtDetail s : sdps){
			s.setPlu_mango("");
			
			String detailSql = sqlpropertyUtil.getValue(store, "hht1034.00.00");
			Object[] skuParam = new Object[]{s.getSku()};
			List<Map> dsc = jdbcTemplateUtil.searchForList(detailSql,skuParam);
			if(dsc!=null&&dsc.size()>0){
				s.setSku_dsc(dsc.get(0).get("SKU_DSC").toString());
			}
			sqls[no] = sqlpropertyUtil.getValue(store, "hht1034.02.01");
			o[no] = new Object[]{s.getSku(),s.getSku_dsc(),store,userId,date,sysId};
			no++;
		}
		super.update(bean.getRequest().getRequestID(), sysId, sqls, o,bean.getUser().getStore(),guid,requestValue,"" );
//		int ret = jdbcTemplateUtil.update(sqls, o);
//		if(ret > 0){
//			//上传至HHTSERVER
//		}
//		String sysId = "";
//		if (bean.getUser() != null) {
//			sysId = store + this.systemUtil.getSysid();
//		} else {
//			sysId = this.systemUtil.getSysid();
//		}
//		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
//		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
//		super.updateForValidation(sqls, o, bean.getUser().getStore(), guid, requestValue, "");
//		
//		if(chgCode!=null&&chgCode.length()>0){
//			//上传至JDA 
//			ChginstExchangeMain jdaMain = new ChginstExchangeMain("hht"+store,chgCode,sysId);
//			jdaMain.execute();
//		}
	}

}

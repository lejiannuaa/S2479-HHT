package com.hola.bs.service.hht;

import java.util.Date;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.SkuSugtOrderDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * 上传下单商品建议（新增）-->JDA
 * @author S2139
 * 2012 Aug 30, 2012 11:56:16 AM 
 */
public class HHT_1035_02 extends BusinessService implements ProcessUnit {

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
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("保存并上传商品下单建议数据, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String date = DateUtils.date2String2(new Date());
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
//		String deleteSql = sqlpropertyUtil.getValue(store, "hht1035.01.01");
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		SkuSugtOrderDetail[] ssods = JsonUtil.getDetail(jsonObject, SkuSugtOrderDetail.class);
		int no = 0;
		String sysId = "";
		if (bean.getUser() != null) {
			sysId = store + this.systemUtil.getSysid();
		} else {
			sysId = this.systemUtil.getSysid();
		}
		String[] batchSqls = new String[ssods.length];
		Object[] o = new Object[ssods.length];
		for(SkuSugtOrderDetail s : ssods){
			s.setPlu_mango("");
			batchSqls[no] = sqlpropertyUtil.getValue(store, "hht1035.01.02");
			o[no] = new Object[]{store,s.getSku(),s.getSku_dsc(),s.getPlu_mango(),s.getSku_order_qty(),userId,date,sysId};
			no++;
		}
		
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		//暂时不写chg表
		super.update(bean.getRequest().getRequestID(), sysId, batchSqls, o, store,guid,requestValue,sysId);
		jdbcTemplateUtil.update(batchSqls, o);
	}

}

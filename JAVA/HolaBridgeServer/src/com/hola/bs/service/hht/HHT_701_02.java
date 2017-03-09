package com.hola.bs.service.hht;

import java.util.Arrays;
import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.json.detailVO.ExtCountDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * 保存抽盘数据 上传抽盘数据
 * @author s2139
 * 2012 Aug 28, 2012 9:51:47 AM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_701_02 extends BusinessService implements ProcessUnit {

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
		MDC.put("stoNo", bean.getRequest().getParameter(configpropertyUtil.getValue("sto")));
		log.info("保存商品抽盘数据, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String date = DateUtils.date2String2(new Date());
		
		String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		ExtCountDetail[] detail = JsonUtil.getDetail(jsonObject, ExtCountDetail.class);
		
//		String[] sqls = new String[1];
//		Object[] args = new Object[1];
//		for(ExtCountDetail e : detail){
//			sqls[0] = sqlpropertyUtil.getValue(store, "hht701.01.02");
//			args[0] = new Object[]{stk_no,store,loc_no,e.getSel_no(),e.getSku(),e.getSku_dsc(),e.getSku_first_qty(),e.getSku_ext_qty(),userId,date,userId,date,sysId};
//			
//		}
		String querySql = sqlpropertyUtil.getValue(store, "hht701.01.03");
		String[] sqls = new String[detail.length];
		Object[] o = new Object[detail.length];
		int no = 0;
		for(ExtCountDetail e : detail){
			String detailSql = sqlpropertyUtil.getValue(store, "hht1034.00.00");
			Object[] skuParam = new Object[]{e.getSku()};
			List<Map> dsc = jdbcTemplateUtil.searchForList(detailSql,skuParam);
			if(dsc!=null&&dsc.size()>0){
				e.setSku_dsc(dsc.get(0).get("SKU_DSC").toString());
			}
			
			
			Object[] qo = new Object[]{stk_no,loc_no,e.getSku(),e.getSel_no()};
			List<Map> headList = jdbcTemplateUtil.searchForList(querySql, qo);
			//若查询结果有值则进行update否则进行insert操作stk_ext_count
			if (headList==null || headList.size()==0){
				sqls[no] = sqlpropertyUtil.getValue(store, "hht701.01.02");
				o[no] = new Object[]{stk_no,store,loc_no,e.getSel_no(),e.getSku(),e.getSku_dsc(),e.getSku_first_qty(),e.getSku_ext_qty(),userId,date,userId,date,sysId};
			}else {
				sqls[no] = sqlpropertyUtil.getValue(store, "hht701.01.04");
				o[no] = new Object[]{store,e.getSku_dsc(),e.getSku_first_qty(),e.getSku_ext_qty(),userId,date,userId,date,sysId,stk_no,loc_no,e.getSku(),e.getSel_no()};
			}
			no++;
		}
		super.update(bean.getRequest().getRequestID(), sysId, sqls, o,bean.getUser().getStore(),guid,requestValue,"" );
		

//		int ret = jdbcTemplateUtil.update(sqls, o);
//		if(ret >0){
//			bean.getResponse().setCode(BusinessService.successcode);
//			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.501.01"));
//		}
	}
}

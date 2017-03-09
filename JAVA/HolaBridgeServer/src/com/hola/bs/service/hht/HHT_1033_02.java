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
import com.hola.bs.json.detailVO.StkCheckDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * 保存柜号检核商品-->bridgeServer 提交至hhtserver
 * @author S2139
 * 2012 Aug 31, 2012 5:36:34 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_1033_02 extends BusinessService implements ProcessUnit {

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
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("保存检核柜号下商品的信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String date = DateUtils.date2String2(new Date());
		
		String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		StkCheckDetail[] scds = JsonUtil.getDetail(jsonObject, StkCheckDetail.class);
		String[] sqls = new String[scds.length+1];
		
		String[] sql1 = new String[1];
		
		sql1[0] = sqlpropertyUtil.getValue(store, "hht1031.01.02");
		
		Object[] o = new Object[scds.length+1];
		Object[] oo = new Object[]{store,loc_no};
		o[0] = new Object[]{sysId,store,loc_no};
		jdbcTemplateUtil.update(sql1, o);
		String sql = sqlpropertyUtil.getValue(store, "hht1033.01.03");
		List<Map> list = jdbcTemplateUtil.searchForList(sql, oo);
		
		
		int no=0;
		for(StkCheckDetail s : scds){
			s.setPlu_mango("");
			String detailSql = sqlpropertyUtil.getValue(store, "hht1034.00.00");
			Object[] skuParam = new Object[]{s.getSku()};
			List<Map> dsc = jdbcTemplateUtil.searchForList(detailSql,skuParam);
			if(dsc!=null&&dsc.size()>0){
				s.setSku_dsc(dsc.get(0).get("SKU_DSC").toString());
			}
			
			sqls[no] = sqlpropertyUtil.getValue(store, "hht1033.01.01");
			o[no] = new Object[]{loc_no,store,s.getSel_no(),s.getSku(),s.getSku_dsc(),s.getPlu_mango(),userId,date,sysId};
			
			for(int i=0;i<list.size();i++){
				if(list.get(i).get("sku").toString().equals(s.getSku())){
					sqls[no] = sqlpropertyUtil.getValue(store, "hht1033.01.04");
					o[no] = new Object[]{sysId,date,s.getSel_no(),s.getSku_dsc(),s.getPlu_mango(),store,loc_no,s.getSku()};
				}
				
				
			}
			
			
			no++;
		}
		super.update(bean.getRequest().getRequestID(), sysId, sqls, o,bean.getUser().getStore(),guid,requestValue,"" );
//		int ret = jdbcTemplateUtil.update(sqls, o);//始终不确定返回的是行数还是0、1结果
//		if(ret >= 0){
//			
//			//此处做数据上传HHTSERVER操作
//			bean.getResponse().setCode(BusinessService.successcode);
//			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1033.01"));
//		}

		
	}

}

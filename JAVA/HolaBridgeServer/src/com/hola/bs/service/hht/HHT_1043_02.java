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
import com.hola.bs.json.detailVO.GroupSkuDetail;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.JsonUtil;

/**
 * 上传提交端架商品资料信息
 * @author S2139
 * 2013 Jan 14, 2013 1:47:52 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_1043_02 extends BusinessService implements ProcessUnit {

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
		log.info("保存并上传端架商品信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean)throws Exception {
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String groupId = bean.getRequest().getParameter(configpropertyUtil.getValue("groupId"));
		
		String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
		com.alibaba.fastjson.JSONObject jsonObject = JsonUtil.analyze(bean.getRequest().getParameter(configpropertyUtil.getValue("json")));
		GroupSkuDetail[] gsds = JsonUtil.getDetail(jsonObject, GroupSkuDetail.class);
		String[] sqls = new String[gsds.length+1];
		Object[] o = new Object[gsds.length+1];
		//全部狀態置D
		String[] sql1 = new String[1];
		sql1[0] = sqlpropertyUtil.getValue(store, "hht1043.02.02");
		o[0] = new String[]{sysId,store,groupId};
		jdbcTemplateUtil.update(sql1, o);
		//查出所有數據進行比對
		String sql = sqlpropertyUtil.getValue(store, "hht1043.02.03");
		Object[] oo = new Object[]{store,groupId};
		List<Map> list = jdbcTemplateUtil.searchForList(sql, oo);
		
		
		
		int no = 0;
		for(GroupSkuDetail g:gsds){
			
			sqls[no] = sqlpropertyUtil.getValue(store, "hht1043.02.01");
			o[no] = new Object[]{groupId,store,g.getSku(),g.getSku_dsc(),g.getSku_qty(),sysId};
			
			for(int i=0;i<list.size();i++){
				if(list.get(i).get("sku").toString().equals(g.getSku())){
					sqls[no] = sqlpropertyUtil.getValue(store, "hht1043.02.04");
					o[no] = new Object[]{sysId,groupId,store,g.getSku()};
				}
				
			}
			
			no++;
		}
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		super.update(bean.getRequest().getRequestID(), sysId, sqls, o, store,guid,requestValue,sysId);
		//jdbcTemplateUtil.update(sqls, o);
	}
	

}

package com.hola.jda2hht.service.impl;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.Logger;

import com.hola.jda2hht.dao.BaseDao;
import com.hola.jda2hht.model.StoreInfoBean;
import com.hola.jda2hht.service.IStoreInfoService;

/**
 * 门店信息的业务实现
 * 
 * @author 唐植超(上海软通)
 * @date 2013-1-4
 */
public class StoreInfoServiceImpl implements IStoreInfoService {

	private static final Logger log = Logger
			.getLogger(StoreInfoServiceImpl.class);
	private BaseDao dao;

	public BaseDao getDao() {
		return dao;
	}

	public void setDao(BaseDao dao) {
		this.dao = dao;
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<StoreInfoBean> getAllStoreList() {
		log.info("查询所有的门店");
		String sql = "select STORENO,AREACODE,STRNAME,STATUS from HOLA_APP_CFG_STOREINFO where status='E'";
		return (List<StoreInfoBean>) this.dao.getList(sql, null,
				StoreInfoBean.class);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<String> checkEnable(List<String> storeStrList) {
		log.info(storeStrList);
		
		String storeIds = "";
		
		for (String st : storeStrList) {
			storeIds += ",'" + st + "'";
		}
		if(storeStrList.size() <= 0)
			return new ArrayList<String>();
		String sql = "select STORENO from HOLA_APP_CFG_STOREINFO where status='E' and STORENO in (" + storeIds.substring(1) + ")";
		List<?> list = this.dao.getJdbcTemplate().queryForList(sql);
		List<String> result = new ArrayList<String>();
		for (Object obj : list) {
			Map<String, String> map = (Map<String, String>) obj;
			result.add(map.get("STORENO"));
		}
		return result;
	}
}

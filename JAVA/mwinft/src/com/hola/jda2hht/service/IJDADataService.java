package com.hola.jda2hht.service;

import java.util.List;

import com.hola.jda2hht.model.ChangeTableDetailBean;

/**
 * JDA的数据查询和封装用这个接口，当然 ，除了那两个表之外
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-28
 */
public interface IJDADataService {

	List<String[]> searchData(String sql, Object[] params,
			List<ChangeTableDetailBean> ctdbList);
	
	List<String[]> searchData(String sql, Object[] params,
			List<ChangeTableDetailBean> ctdbList, String table);

	int searchScanlar(String sql);

	List<String> searchStoreByInstNo(String instno, String dbschema,
			String srcnam, String colName);

}

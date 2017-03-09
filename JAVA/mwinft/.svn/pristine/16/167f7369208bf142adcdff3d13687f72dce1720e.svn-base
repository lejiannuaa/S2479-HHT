package com.hola.jda2hht.service.impl;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.model.ChangeTableDetailBean;
import com.hola.jda2hht.service.IJDADataService;

public class JDADataService extends BaseJDAService implements IJDADataService {

	public JDADataService(ChangeSysBean sysBean) throws Exception {
		super(sysBean);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<String[]> searchData(String sql, Object[] params,
			List<ChangeTableDetailBean> ctdbList) {
		List<?> list = this.dao.getJdbcTemplate().queryForList(sql, params);
		List<String[]> result = new LinkedList<String[]>();
		for (Object obj : list) {
			Map<String, Object> map = (Map<String, Object>) obj;
			String[] array = new String[ctdbList.size()];
			int i = 0;
			for (ChangeTableDetailBean ctdb : ctdbList) {
				array[i++] = map.get(ctdb.getSrcfldcode()) + "";

			}
			result.add(array);
		}
		return result;
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public List<String[]> searchData(String sql, Object[] params,
			List<ChangeTableDetailBean> ctdbList, String table) {
		List<?> list = this.dao.getJdbcTemplate().queryForList(sql, params);
		List<String[]> result = new LinkedList<String[]>();
		for (Object obj : list) {
			Map<String, Object> map = (Map<String, Object>) obj;
			String[] array = new String[ctdbList.size()];
			int i = 0;
			/**如果SRC =SOM ，且批次Tar 是JDA，更改chginst.chgtyp o为I
			 * */
			if (table.lastIndexOf("CHGINST")!= -1 ) {

				String src = (String)map.get("SRCUSR");
				String tar = (String)map.get("TRGSVR");
				if (!"".equals(src) && null != src  && !"".equals(tar) && null != tar) {
					if ("SOM".equals(src.trim()) && "JDA".equals(tar.trim())){
						map.put("CHGTYP", "I");
					}
				}
				
			}
			

			
			for (ChangeTableDetailBean ctdb : ctdbList) {
				
				array[i++] = map.get(ctdb.getSrcfldcode()) + "";
			}
			result.add(array);
		}
		return result;
	}

	@Override
	public int searchScanlar(String sql) {
		return this.dao.getJdbcTemplate().queryForInt(sql);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<String> searchStoreByInstNo(String instno, String dbschema,
			String srcnam, String colName) {
		String sql = "select distinct(" + colName.toUpperCase() + ") from "
				+ dbschema + "." + srcnam + " where instno=?";

		List<?> list = this.dao.getJdbcTemplate().queryForList(sql,
				new Object[] { instno });
		List<String> result = new ArrayList<String>();
		for (Object obj : list) {
			Map<String, String> map = (Map<String, String>) obj;
			result.add(map.get(colName.toUpperCase()).trim());
		}
		return result;
	}
}

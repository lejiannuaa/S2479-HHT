package com.hola.common.dao;

import java.util.List;
import java.util.Map;

import org.springframework.jdbc.core.JdbcTemplate;

public class BaseDao 
{
	private		JdbcTemplate		storeJt;
	private		JdbcTemplate		hhtserverJt;
	private		Map<String, String> updateViewSqlForChgcode;
	
	public JdbcTemplate getStoreJt() {
		return storeJt;
	}
	public void setStoreJt(JdbcTemplate storeJt) {
		this.storeJt = storeJt;
	}
	public JdbcTemplate getHhtserverJt() {
		return hhtserverJt;
	}
	public void setHhtserverJt(JdbcTemplate hhtserverJt) {
		this.hhtserverJt = hhtserverJt;
	}
	@SuppressWarnings("rawtypes")
	protected int getCount(List list) 
	{
		int count = 0;
		for (Object object : list) 
		{
			Map map = (Map)object;
			Object obj = map.get("COUNTNUM");
			if(obj != null)
			{
				if(obj instanceof Long)
					count = ((Long)map.get("COUNTNUM")).intValue();
				else if(obj instanceof Integer)
					count = ((Integer)map.get("COUNTNUM")).intValue();
			}
		}
		return count;
	}
	protected String createSqlForCount(String sql) 
	{
		sql = "select count(*) as COUNTNUM from (" + sql + ") as t";
		return sql;
	}
	public Map<String, String> getUpdateViewSqlForChgcode() {
		return updateViewSqlForChgcode;
	}
	public void setUpdateViewSqlForChgcode(
			Map<String, String> updateViewSqlForChgcode) {
		this.updateViewSqlForChgcode = updateViewSqlForChgcode;
	}
}

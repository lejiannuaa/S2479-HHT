package com.hola.tarbs.service.execution;

import java.util.ArrayList;
import java.util.List;

import com.hola.common.model.mw.ChgTbl;

public class DelForBs extends Executor 
{

	@Override
	public List<String> sqlCreate(Object data, Object config, String schema,
			String instno) 
	{
		StringBuffer sql = new StringBuffer();
		ChgTbl chgtbl = (ChgTbl) config;
		String [] datas = (String[]) data;
		super.createDel(datas, chgtbl, schema, sql);
		List<String> sqls = new ArrayList<String>();
		sqls.add(sql.toString());
		return sqls;
	}

}

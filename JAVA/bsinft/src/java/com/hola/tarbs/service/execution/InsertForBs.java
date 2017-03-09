package com.hola.tarbs.service.execution;

import java.util.ArrayList;
import java.util.List;

import com.hola.common.model.mw.ChgTbl;

public class InsertForBs extends Executor 
{

	@Override
	public List<String> sqlCreate(Object data, Object config, String schema,
			String instno) 
    {
		StringBuffer sql = new StringBuffer();
		ChgTbl chgtbl = (ChgTbl) config;
		String [] datas = (String[]) data;
		super.createInsert(schema, datas, chgtbl, sql, instno);	
		List<String> sqls = new ArrayList<String>(1);
		sqls.add(sql.toString());
		return sqls;
	}

}

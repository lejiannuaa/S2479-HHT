package com.hola.bs.impl;

import java.util.List;

public interface JdbcTemplateSqlServerUtil2 {
	public List searchForList(String sql) ;
	
	public List searchForList(String sql,Object[] ages)  ;
	
//	public boolean update(String sql[])  throws Exception;
	
	public int update(String[] sql,Object[] ages) ;
}

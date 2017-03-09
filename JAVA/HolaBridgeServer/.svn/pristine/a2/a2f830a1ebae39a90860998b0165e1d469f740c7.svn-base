package com.hola.bs.service;

import java.util.Map;
import java.util.Set;

public interface IChginstDao {
	public Map<String, Integer> queryCount(String[] sql, String view,
			Set<String> tables, Object[] ages);

	public String rebuildUpdateToSelect(String update);

	public int getSelectCount(String sql, Object[] ages);

	public String getChgCode(String commandId) ;

	public Map<String, Integer> queryCount(Set<String> tables, String instno);

	public void execute(String commandId, String instno, String[] sqlArr,
			Object[] ages, String store);
}

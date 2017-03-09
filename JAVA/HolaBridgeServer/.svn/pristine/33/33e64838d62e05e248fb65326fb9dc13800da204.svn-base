package com.hola.bs.service;

import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.impl.JdbcTemplateSqlServerUtil2;

public class JdbcTemplateSqlServerImpl2 implements JdbcTemplateSqlServerUtil2{
public Log log = LogFactory.getLog(getClass());
	
	@Autowired(required = true)
	private JdbcTemplate jdbcTemplateSqlServer2;
	
	public JdbcTemplate getJdbcTemplateSqlServer() {
		return jdbcTemplateSqlServer2;
	}

	public void setJdbcTemplateSqlServer(JdbcTemplate jdbcTemplateSqlServer2) {
		this.jdbcTemplateSqlServer2 = jdbcTemplateSqlServer2;
	}

	public List searchForList(String sql, Object[] ages) {
		if (sql != null) {
			log.info("执行语句=" + sql);
		}
		if (ages != null) {
			for (Object o : ages) {
				if (o instanceof Object[]) {
					Object[] oo = (Object[]) o;
					String s = "";
					for (Object o1 : oo) {
						s += o1 + "\t";
					}
					log.info("参数=" + s);
				} else {
					log.info("参数=" + o);
				}
			}
		}

		return jdbcTemplateSqlServer2.queryForList(sql, ages);
	}

	public List searchForList(String sql) {
		if (sql != null) {
			log.info("执行语句=" + sql);
		}
		return jdbcTemplateSqlServer2.queryForList(sql);
	}

	public int update(String[] sql) {
		int total = 0;
			for (String s : sql) {
				total += jdbcTemplateSqlServer2.update(s);
			}
		return total;
	}

	public int update(String[] sql, Object[] args)  {
		if (sql != null) {
			for (String s : sql)
				log.info("执行语句=" + s);
		}
		if (args != null) {
			for (Object o : args) {
				if (o instanceof Object[]) {
					Object[] oo = (Object[]) o;
					String s = "";
					for (Object o1 : oo) {
						s += o1 + "\t";
					}
					log.info("参数=" + s);
				} else {
					log.info("参数=" + o);
				}
			}
		}
		int total = 0;
		if (args == null)
			return update(sql);
			int i = 0;
			for (String s : sql) {
				// Object[] o = (Object[]) (args[0]);
				System.out.println((Object[]) (args[i]));
				if (s != null && s.length() > 0) {
					total += jdbcTemplateSqlServer2.update(s, (Object[]) args[i]);
				}
				i++;
			}
		return total;
	}
}

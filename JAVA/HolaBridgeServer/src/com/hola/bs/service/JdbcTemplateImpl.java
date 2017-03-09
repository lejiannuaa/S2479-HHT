package com.hola.bs.service;

import java.util.List;
import java.util.Map;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.impl.JdbcTemplateUtil;

public class JdbcTemplateImpl implements JdbcTemplateUtil {
	public Log log = LogFactory.getLog(getClass());

	@Autowired(required = true)
	private JdbcTemplate jdbcTemplate;

	public JdbcTemplate getJdbcTemplate() {
		return jdbcTemplate;
	}

	public void setJdbcTemplate(JdbcTemplate jdbcTemplate) {
		this.jdbcTemplate = jdbcTemplate;
	}

	public List<Map<String,Object>> searchForList(String sql, Object[] ages) {
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

		return jdbcTemplate.queryForList(sql, ages);
	}

	public List<Map<String,Object>> searchForList(String sql) {
		if (sql != null) {
			log.info("执行语句=" + sql);
		}
		return jdbcTemplate.queryForList(sql);
	}

	public int update(String[] sql) {
		int total = 0;
			for (String s : sql) {
				total += jdbcTemplate.update(s);
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
				if (s != null && s.length() > 0) {
					total += jdbcTemplate.update(s, (Object[]) args[i]);
				}
				i++;
			}
		return total;
	}
}

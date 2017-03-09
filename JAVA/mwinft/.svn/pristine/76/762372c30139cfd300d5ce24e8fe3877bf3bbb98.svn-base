package com.hola.jda2hht.dao;

import java.util.List;

import org.springframework.jdbc.core.BeanPropertyRowMapper;
import org.springframework.jdbc.core.JdbcTemplate;

public class BaseDao {

	private JdbcTemplate jdbcTemplate;

	public void execute(String sql) {
		jdbcTemplate.execute(sql);
	}

	public void update(String sql, Object[] params) {
		jdbcTemplate.update(sql, params);
	}

	public JdbcTemplate getJdbcTemplate() {
		return jdbcTemplate;
	}

	public void setJdbcTemplate(JdbcTemplate jdbcTemplate) {
		this.jdbcTemplate = jdbcTemplate;
	}

	@SuppressWarnings("unchecked")
	public <T> T getObject(String sql, Object[] params, Class<T> clzz) {
		return (T) this.jdbcTemplate.queryForObject(sql, params,
				new BeanPropertyRowMapper(clzz));
	}

	public List<?> getList(String sql, Object[] params, Class<?> clzz) {
		return this.jdbcTemplate.query(sql, params, new BeanPropertyRowMapper(
				clzz));
	}

}

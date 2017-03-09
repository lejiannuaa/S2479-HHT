package com.hola.jda2hht.dao;

import java.io.PrintWriter;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.SQLFeatureNotSupportedException;
import java.util.logging.Logger;

import javax.sql.DataSource;

/**
 * 自定义datasouce的实现，不解释，因为里面的内容很简单
 * 
 * @author 唐植超(上海软通)
 * @date 2013-1-5
 */
public class DB2DataSouce implements DataSource {

	private String driver;
	private String url;
	private String name;
	private String pass;

	// 没有用数据库连接池，只是简单的实习了一下，如果需要改进性能，这里可以优化
	// 毕竟我只有10天时间，没空改这个
	public DB2DataSouce(String driver, String url, String name, String pass)
			throws Exception {
		super();
		this.driver = driver;
		this.url = url;
		this.name = name;
		this.pass = pass;
		Class.forName(this.driver);
	}

	@Override
	public PrintWriter getLogWriter() throws SQLException {
		return null;
	}

	@Override
	public void setLogWriter(PrintWriter out) throws SQLException {

	}

	@Override
	public void setLoginTimeout(int seconds) throws SQLException {

	}

	@Override
	public int getLoginTimeout() throws SQLException {

		return 0;
	}

	@SuppressWarnings("unchecked")
	@Override
	public <T> T unwrap(Class<T> iface) throws SQLException {
		return (T) this;
	}

	@Override
	public boolean isWrapperFor(Class<?> iface) throws SQLException {
		return DataSource.class.equals(iface);
	}

	@Override
	public Connection getConnection() throws SQLException {
		return this.getConnection(name, pass);
	}

	@Override
	public Connection getConnection(String username, String password)
			throws SQLException {
		Connection conn = DriverManager.getConnection(url, username, password);
		conn.setAutoCommit(true);
		return conn;
	}

	public Logger getParentLogger() throws SQLFeatureNotSupportedException {
		// TODO Auto-generated method stub
		return null;
	}

}

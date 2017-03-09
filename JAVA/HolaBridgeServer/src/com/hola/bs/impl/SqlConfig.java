package com.hola.bs.impl;

public class SqlConfig {

    public static String get(String id) {
        return "SELECT ORDERPLANID SKU,VENDOR 品名 from HOLA_APP_DC_OP where ORDERPLANID = 3102";
    }

}

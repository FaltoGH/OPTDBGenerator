# OPTDBGenerator
## Specifications
- .NET Framework 4.8 Console Application
- Using [AxKHOpenAPI](https://www1.kiwoom.com/h/customer/download/VOpenApiInfoView?dummyVal=0)
- Using [System.Data.SQLite](https://system.data.sqlite.org/)
- CREATE TABLE '{jmcode}'(현재가 INTEGER,
거래량 INTEGER,
거래대금 INTEGER,
일자 TEXT PRIMARY KEY,
시가 INTEGER,
고가 INTEGER,
저가 INTEGER,
수정주가구분 INTEGER,
수정비율 TEXT)
- CREATE TABLE IF NOT EXISTS '{jmcode}'(일자 TEXT PRIMARY KEY,
현재가 INTEGER,
대비기호 INTEGER,
전일대비 INTEGER,
등락율 TEXT,
누적거래량 INTEGER,
누적거래대금 INTEGER,
개인투자자B INTEGER,
외국인투자자B INTEGER,
기관계B INTEGER,
금융투자B INTEGER,
보험B INTEGER,
투신B INTEGER,
기타금융B INTEGER,
은행B INTEGER,
연기금등B INTEGER,
사모펀드B INTEGER,
국가B INTEGER,
기타법인B INTEGER,
내외국인B INTEGER,
개인투자자S INTEGER,
외국인투자자S INTEGER,
기관계S INTEGER,
금융투자S INTEGER,
보험S INTEGER,
투신S INTEGER,
기타금융S INTEGER,
은행S INTEGER,
연기금등S INTEGER,
사모펀드S INTEGER,
국가S INTEGER,
기타법인S INTEGER,
내외국인S INTEGER)
- Do not request already existing data
- Remove unnecessary jmcode
- Remove unnecessary date

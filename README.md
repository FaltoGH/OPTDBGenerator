# OPTDBGenerator
## Specifications
- CREATE TABLE '{jmcode}'(현재가 INTEGER,
거래량 INTEGER,
거래대금 INTEGER,
일자 TEXT PRIMARY KEY,
시가 INTEGER,
고가 INTEGER,
저가 INTEGER,
수정주가구분 INTEGER,
수정비율 TEXT)
- Skip requesting opt10081 data if DB already has last 600 dates' daily chart of that code
- Drop opt10081 table if the table name is not contained by common codes

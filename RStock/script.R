library(quantmod)
stock <- getSymbols("1234.TW", auto.assign = FALSE)
chartSeries(stock["2017-01-11::2018-01-11"])
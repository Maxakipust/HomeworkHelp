#ifndef _DB_H
#define _DB_H

#include <sqlite.h>

#define DB_PATH "homework.db"

typedef sqlite DB;

typedef struct
{
  char **list;
  int nrow;
  int ncolumn;
} DB_Resp;

extern void DB_FreeResp(DB_Resp *list);
extern DB *DB_Open();
extern void DB_Close(DB *db);
/* please validate inputs! */
extern DB_Resp *DB_Query(DB *db, const char *format, ...);

#endif

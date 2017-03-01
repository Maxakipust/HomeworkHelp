#include "db.h"
#include <stdlib.h>
#include <string.h>

void DB_FreeResp(DB_Resp *resp)
{
  if(resp == NULL)
    return;
  if(resp->list != NULL)
    sqlite_free_table(resp->list);
  free(resp);
}

DB *DB_Open()
{
  sqlite_open(DB_PATH, 0, NULL);
}

void DB_Close(DB *db)
{
  sqlite_close(db);
}

DB_Resp *DB_Query(DB *db, const char *format, ...)
{
  va_list ap;
  DB_Resp *resp = (DB_Resp*)malloc(sizeof(DB_Resp));
  memset(resp, 0, sizeof(DB_Resp));
  va_start(ap, format);
  if(sqlite_get_table_vprintf(db, format, &resp->list, &resp->nrow, &resp->ncolumn, NULL, ap) != SQLITE_OK)
  {
    DB_FreeResp(resp);
    return NULL;
  }
  va_end(ap);
  return resp;
}

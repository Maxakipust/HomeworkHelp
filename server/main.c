#include "../lib/secure/secure.h"
#include "db.h"

#define PORT 8910

int LoadKeys(DB *db, Secure_PubKey pub, Secure_PrivKey priv);
int NewKeys(DB *db, Secure_PubKey pub, Secure_PrivKey priv);

int main(int argv, char **argc)
{
  Secure_Server server = Secure_StartServer(PORT);

  DB *db = DB_Open();

  Secure_PubKey pub = Secure_NewPubKey();
  Secure_PrivKey priv = Secure_NewPrivKey();

  if(LoadKeys(db, pub, priv))
  {
    Secure_FreePubKey(pub);
    Secure_FreePrivKey(priv);
    DB_Close(db);
    return -1;
  }

  while(getchar() != 'q') ;//TODO

  return 0;
}

int LoadKeys(DB *db, Secure_PubKey pub, Secure_PrivKey priv)
{
  #define KEY_TABLE "keys"

  DB_Resp *resp;

  //resp = DB_Query(db, "SELECT * FROM sqlite_master WHERE type='table' AND name='%s';",
  // KEYTABLE);

  if(resp == NULL || resp->nrow == 0)
  {
    if(NewKeys(db, pub, priv))
      return -1;
  }

  return 0;
}

int NewKeys(DB *db, Secure_PubKey pub, Secure_PrivKey priv)
{
    DB_FreeResp(DB_Query(db, "CREATE TABLE (pubkey BINARY(%d), privkey BINARY(%d));"));

    Secure_GenKeys(pub, priv);

    DB_FreeResp(DB_Query(db, "INSERT INTO %s (pubkey, privkey) VALUES ('%q', '%q');"));

    return 0;
}

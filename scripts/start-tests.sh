#!/bin/sh

source ./scripts/apply-migrations.sh

dotnet test tests/ScheduleBarbecue.Tests/ --configuration Release --logger trx --logger "console;verbosity=normal"
#!/usr/bin/env sh

target="Publish a game or Bust"
date=`date "+%Y-%m-%d_%H-%M-%S"`
bkpfile="PAGOB_$date.tar.gz"

cd ..

echo "Compressing ${target} into $bkpfile"

tar cf "${bkpfile}" "${target}"

scp "${bkpfile}" h3nnn4n@clusterfuck.servequake.com:~/gamedev_bkps/

cd -

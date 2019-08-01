#!/bin/bash
echo "*°*°*°* Creazione dispositivo Iot AWS*°*°*°*"

ROOT_CERTIF="./"
ROOT_FILE="./"
QUANTITY=""
DEVICE_PREFIX=""

if [ -z "$QUANTITY" ]; then
  echo -e "Numero device da creare: "
  read QUANTITY
fi

if [ -z "$DEVICE_PREFIX" ]; then
  echo -e "Nome per device: "
  read DEVICE_PREFIX
  echo "export DEVICE_PREFIX=$DEVICE_PREFIX">>$CONF
fi

echo "*°*°*°* Creo certificati e li salvo in ${ROOT_CERTIF}*°*°*°*"
aws iot create-keys-and-certificate --set-as-active \
         --public-key-outfile $ROOT_CERTIF/1.public.key \
         --private-key-outfile $ROOT_CERTIF/2.private.key \
         --certificate-pem-outfile $ROOT_CERTIF/3.certificate.pem > $ROOT_CERTIF/out --output json
echo "*°*°*°* Creo variabile ARN dei certificati *°*°*°*"
ARN=$(jq -r '.certificateArn' $ROOT_CERTIF/out)
echo "export ARN=$ARN"

for i in `seq 1 ${QUANTITY}`; do
    DEVICE=${DEVICE_PREFIX}${i}
    echo "*°*°*°* Creating device: ~~~> ${DEVICE}"
    #Create the device
    aws iot create-thing --thing-name $DEVICE >> $ROOT_FILE/devicelist || exit 1
    #Assegno certificato a device
    aws iot attach-thing-principal --thing-name $DEVICE --principal $ARN || exit 2
done

echo "*°*°*°* DEVICE CREATION SUCCESSFULL *°*°*°*"
cat $ROOT_FILE/devicelist


if [ -z "$POLICYNAME" ]; then
  echo -e "Nome per Policy: "
  read POLICYNAME
  echo "export POLICYNAME=$POLICYNAME"
fi

echo "*°*°*°* Creazione policy AWSIOT *°*°*°*"
aws iot create-policy --policy-name $POLICYNAME --policy-document '{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": "iot:*",
      "Resource": "*"
    }
  ]
}' >> $ROOT_FILE/policy

aws iot attach-policy --policy-name $POLICYNAME --target $ARN
cat $ROOT_FILE/policy
echo "*°*°*°* Policy collegata a certificato *°*°*°*"


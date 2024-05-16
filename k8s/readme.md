# Readme


## Commands
Update FluentBit Config
```
kubectl delete ds -n bbl-poc bbl-poc-daemonset-fluent-bit && kubectl apply -f k8s/configmap.yaml && kubectl apply -f k8s/daemonset.yaml && sleep 3 && kubectl get all -n bbl-poc
```
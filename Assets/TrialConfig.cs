using System.Text;

public interface TrialConfig {
    public StringBuilder SerializeLabels();
    public StringBuilder SerializeFields();
}
public class SimpleTrialConfig : TrialConfig {
    public float feature;
    public StringBuilder SerializeLabels() {
        return TypeHelpers.SerializeFieldLabels<SimpleTrialConfig>();
    }
    public StringBuilder SerializeFields() {
        return TypeHelpers.SerializeFields<SimpleTrialConfig>(this);
    }
    public override string ToString() {
        return $"{SerializeLabels()} = {SerializeFields()}";
    }
    public static TrialConfig CreateFromFeatureInput(float feature) {
        return new SimpleTrialConfig {feature=feature};
    }
    public static TrialConfig CreateDummyTrialConfig(int seed) {
        return new SimpleTrialConfig {feature=(float) seed};
    }
}